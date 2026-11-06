using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecNM.Research.Common;
using TecNM.Research.Data;
using TecNM.Research.Entities;
using TecNM.Research.Infrastructure;

namespace TecNM.Research.Features.Documents;

[ApiController]
[Route("api/v1/document-templates")]
public class TemplatesController(TemplatesService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> List([FromQuery] TemplatesQuery query, CancellationToken ct) => Ok(await service.ListAsync(query, ct));
    [HttpGet("{id:long:min(1)}")] public async Task<IActionResult> Get(long id, CancellationToken ct) => Ok(await service.GetAsync(id, ct));
    [HttpGet("versions/{code}")] public async Task<IActionResult> Versions(string code, [FromQuery] PageQuery query, CancellationToken ct) => Ok(await service.VersionsAsync(code, query, ct));
    [HttpPost, Consumes("multipart/form-data")]
    [RequestSizeLimit(32L * 1024 * 1024)]
    public async Task<IActionResult> Upload([FromForm] TemplateUploadRequest request, CancellationToken ct)
    {
        var result = await service.UploadAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    [HttpPut("{id:long:min(1)}")] public async Task<IActionResult> Update(long id, TemplateUpdateRequest request, CancellationToken ct) => Ok(await service.UpdateAsync(id, request, ct));
    [HttpPatch("{id:long:min(1)}/current")] public async Task<IActionResult> Current(long id, CancellationToken ct) { await service.MakeCurrentAsync(id, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/active")] public async Task<IActionResult> Active(long id, ActiveRequest request, CancellationToken ct) { await service.StateAsync(id, request.IsActive!.Value, null, false, false, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/visibility")] public async Task<IActionResult> Visibility(long id, VisibilityRequest request, CancellationToken ct) { await service.StateAsync(id, null, request.IsVisible!.Value, false, false, ct); return NoContent(); }
    [HttpDelete("{id:long:min(1)}")] public async Task<IActionResult> Delete(long id, CancellationToken ct) { await service.StateAsync(id, null, null, true, false, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/restore")] public async Task<IActionResult> Restore(long id, CancellationToken ct) { await service.StateAsync(id, null, null, false, true, ct); return NoContent(); }
    [HttpGet("{id:long:min(1)}/download")]
    public async Task<IActionResult> Download(long id, CancellationToken ct)
    {
        var file = await service.DownloadAsync(id, ct);
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        return File(file.Stream, file.MediaType, file.Name);
    }
}
public class TemplateUploadRequest
{
    [Required, StringLength(80)] public string Code { get; set; } = "";
    [Required, StringLength(300)] public string Name { get; set; } = "";
    [Required] public IFormFile File { get; set; } = null!;
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}
// El trigger permite cambiar solo estos metadatos; nombre, código y archivo son inmutables.
public class TemplateUpdateRequest
{
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}
public class TemplatesQuery : PageQuery { public bool CurrentOnly { get; set; } = true; }
public record TemplateResponse(long Id, string Code, string Name, string OriginalFileName, string MediaType, long SizeBytes, string Sha256, int VersionNumber, bool IsCurrent, bool IsActive, bool IsVisible, int DisplayOrder, string? Notes, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);

public class TemplatesService(AppDbContext db, ICurrentActor actor, ILocalFileStorage storage)
{
    private IQueryable<TemplateResponse> Read(bool includeDeleted = false) => db.CDocumentTemplates.AsNoTracking().Where(t => includeDeleted || t.DeletedAt == null).OrderBy(t => t.Code).ThenByDescending(t => t.VersionNumber).Select(t => new TemplateResponse(t.Id, t.Code, t.Name, t.OriginalFileName, t.MediaType, t.SizeBytes, t.Sha256, t.VersionNumber, t.IsCurrent, t.IsActive, t.IsVisible, t.DisplayOrder, t.Notes, t.CreatedAt, t.UpdatedAt, t.DeletedAt));
    public async Task<PageResponse<TemplateResponse>> ListAsync(TemplatesQuery query, CancellationToken ct)
    {
        var q = Read(query.IncludeDeleted);
        if (query.CurrentOnly) q = q.Where(t => t.IsCurrent);
        if (!string.IsNullOrWhiteSpace(query.Search)) { var term = query.Search.Trim().ToLowerInvariant(); q = q.Where(t => t.Code.ToLower().Contains(term) || t.Name.ToLower().Contains(term)); }
        return await q.ToPageAsync(query, ct);
    }
    public Task<PageResponse<TemplateResponse>> VersionsAsync(string code, PageQuery query, CancellationToken ct) => Read(query.IncludeDeleted).Where(t => t.Code == RecordActions.Code(code)).ToPageAsync(query, ct);
    public async Task<TemplateResponse> GetAsync(long id, CancellationToken ct) => await Read().SingleOrDefaultAsync(t => t.Id == id, ct) ?? throw new ApiFault(404, "Plantilla no encontrada.");

    // Serializa cambios por código en PostgreSQL y respeta el índice de una versión vigente.
    private Task<int> LockAsync(string code, CancellationToken ct) => db.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtextextended({code}, 0))", ct);
    private async Task SelectCurrentAsync(string code, long? id, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        await db.CDocumentTemplates.Where(t => t.Code == code && t.IsCurrent && t.DeletedAt == null).ExecuteUpdateAsync(u => u.SetProperty(t => t.IsCurrent, false).SetProperty(t => t.UpdatedAt, now).SetProperty(t => t.UpdatedBy, actor.Id), ct);
        if (id.HasValue) await db.CDocumentTemplates.Where(t => t.Id == id && t.DeletedAt == null).ExecuteUpdateAsync(u => u.SetProperty(t => t.IsCurrent, true).SetProperty(t => t.UpdatedAt, now).SetProperty(t => t.UpdatedBy, actor.Id), ct);
        // Los tipos que siguen a este código apuntan siempre a la versión vigente.
        var groupIds = db.CDocumentTemplates.Where(t => t.Code == code).Select(t => t.Id);
        if (id.HasValue) await db.CDocumentTypes.Where(t => t.TemplateId.HasValue && groupIds.Contains(t.TemplateId.Value)).ExecuteUpdateAsync(u => u.SetProperty(t => t.TemplateId, id).SetProperty(t => t.UpdatedAt, now).SetProperty(t => t.UpdatedBy, actor.Id), ct);
    }
    public async Task<TemplateResponse> UploadAsync(TemplateUploadRequest request, CancellationToken ct)
    {
        Paging.Validate(request);
        var code = RecordActions.Code(request.Code);
        var file = await storage.SaveAsync(request.File, ct);
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        await LockAsync(code, ct);
        var version = (await db.CDocumentTemplates.Where(t => t.Code == code).MaxAsync(t => (int?)t.VersionNumber, ct) ?? 0) + 1;
        // Desmarcar primero evita violar el índice único inmediato.
        await SelectCurrentAsync(code, null, ct);
        var t = new CDocumentTemplates { Code = code, Name = request.Name.Trim(), OriginalFileName = file.OriginalName, StorageKey = file.Key, MediaType = file.MediaType, SizeBytes = file.Size, Sha256 = file.Sha256, VersionNumber = version, IsCurrent = true, IsActive = true, IsVisible = true, DisplayOrder = request.DisplayOrder, Notes = RecordActions.Clean(request.Notes) };
        RecordActions.Created(t, actor.Id);
        db.CDocumentTemplates.Add(t);
        await db.SaveChangesAsync(ct);
        var groupIds = db.CDocumentTemplates.Where(x => x.Code == code).Select(x => x.Id);
        await db.CDocumentTypes.Where(x => x.TemplateId.HasValue && groupIds.Contains(x.TemplateId.Value)).ExecuteUpdateAsync(u => u.SetProperty(x => x.TemplateId, (long?)t.Id).SetProperty(x => x.UpdatedAt, DateTime.UtcNow).SetProperty(x => x.UpdatedBy, actor.Id), ct);
        await transaction.CommitAsync(ct);
        return await GetAsync(t.Id, ct);
    }
    public async Task<TemplateResponse> UpdateAsync(long id, TemplateUpdateRequest request, CancellationToken ct)
    {
        Paging.Validate(request);
        var t = await RecordActions.FindAsync<CDocumentTemplates>(db, id, ct);
        t.DisplayOrder = request.DisplayOrder; t.Notes = RecordActions.Clean(request.Notes);
        RecordActions.Updated(t, actor.Id);
        await db.SaveChangesAsync(ct);
        return await GetAsync(id, ct);
    }
    public async Task MakeCurrentAsync(long id, CancellationToken ct)
    {
        var code = await db.CDocumentTemplates.Where(t => t.Id == id && t.DeletedAt == null).Select(t => t.Code).SingleOrDefaultAsync(ct) ?? throw new ApiFault(404, "Plantilla no encontrada.");
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        await LockAsync(code, ct);
        if (!await db.CDocumentTemplates.AnyAsync(t => t.Id == id && t.IsActive && t.DeletedAt == null, ct)) throw new ApiFault(409, "La versión debe estar activa y no eliminada.");
        await SelectCurrentAsync(code, id, ct);
        await transaction.CommitAsync(ct);
    }
    public async Task StateAsync(long id, bool? active, bool? visible, bool delete, bool restore, CancellationToken ct)
    {
        var code = await db.CDocumentTemplates.Where(t => t.Id == id).Select(t => t.Code).SingleOrDefaultAsync(ct) ?? throw new ApiFault(404, "Plantilla no encontrada.");
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        await LockAsync(code, ct);
        var t = await RecordActions.FindAsync<CDocumentTemplates>(db, id, ct, restore);
        if (restore)
        {
            if (t.DeletedAt == null) return;
            RecordActions.Restore(t, actor.Id);
            t.IsCurrent = !await db.CDocumentTemplates.AnyAsync(x => x.Code == code && x.IsCurrent && x.DeletedAt == null, ct);
        }
        else if (delete)
        {
            if (t.IsCurrent && await db.CDocumentTemplates.AnyAsync(x => x.Code == code && x.Id != id && x.DeletedAt == null, ct)) throw new ApiFault(409, "Selecciona otra versión vigente antes de eliminar esta versión.");
            if (await db.CDocumentTypes.AnyAsync(x => x.TemplateId == id, ct)) throw new ApiFault(409, "Desvincula los tipos de documento antes de eliminar esta plantilla.");
            t.IsCurrent = false;
            RecordActions.Delete(t, actor.Id);
        }
        else if (active.HasValue)
        {
            if (!active.Value && await db.CDocumentTypes.AnyAsync(x => x.TemplateId == id && x.IsActive && x.DeletedAt == null, ct)) throw new ApiFault(409, "La versión es utilizada por tipos activos. Cambia su plantilla antes de desactivarla.");
            RecordActions.SetActive(t, active.Value, actor.Id);
        }
        else if (visible.HasValue) RecordActions.SetVisible(t, visible.Value, actor.Id);
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }
    public async Task<DownloadFile> DownloadAsync(long id, CancellationToken ct)
    {
        var t = await db.CDocumentTemplates.AsNoTracking().SingleOrDefaultAsync(t => t.Id == id && t.DeletedAt == null, ct) ?? throw new ApiFault(404, "Plantilla no encontrada.");
        return new(storage.Open(t.StorageKey), t.OriginalFileName, t.MediaType);
    }
}
