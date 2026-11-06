using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecNM.Research.Common;
using TecNM.Research.Data;
using TecNM.Research.Entities;

namespace TecNM.Research.Features.Documents;

[ApiController]
[Route("api/v1/document-types")]
public class DocumentTypesController(DocumentTypesService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> List([FromQuery] PageQuery query, CancellationToken ct) => Ok(await service.ListAsync(query, ct));
    [HttpGet("options")] public async Task<IActionResult> Options(CancellationToken ct) => Ok(await service.OptionsAsync(ct));
    [HttpGet("{id:long:min(1)}")] public async Task<IActionResult> Get(long id, CancellationToken ct) => Ok(await service.GetAsync(id, ct));
    [HttpPost] public async Task<IActionResult> Create(DocumentTypesCreateRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    [HttpPut("{id:long:min(1)}")] public async Task<IActionResult> Update(long id, DocumentTypesUpdateRequest request, CancellationToken ct) => Ok(await service.UpdateAsync(id, request, ct));
    [HttpPatch("{id:long:min(1)}/active")] public async Task<IActionResult> Active(long id, ActiveRequest request, CancellationToken ct) { await service.ActiveAsync(id, request.IsActive!.Value, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/visibility")] public async Task<IActionResult> Visibility(long id, VisibilityRequest request, CancellationToken ct) { await service.VisibilityAsync(id, request.IsVisible!.Value, ct); return NoContent(); }
    [HttpDelete("{id:long:min(1)}")] public async Task<IActionResult> Delete(long id, CancellationToken ct) { await service.DeleteAsync(id, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/restore")] public async Task<IActionResult> Restore(long id, CancellationToken ct) { await service.RestoreAsync(id, ct); return NoContent(); }
}

public class DocumentTypesUpdateRequest
{
    [Required, StringLength(80)] public string Code { get; set; } = "";
    [Required, StringLength(250)] public string Name { get; set; } = "";
    public string? Description { get; set; }
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
    [Range(1, long.MaxValue)] public long? TemplateId { get; set; }
}
public sealed class DocumentTypesCreateRequest : DocumentTypesUpdateRequest { }
public record DocumentTypesResponse(long Id, string Code, string Name, string? Description, long? TemplateId, string? TemplateName, bool IsActive, bool IsVisible, int DisplayOrder, string? Notes, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);

public class DocumentTypesService(AppDbContext db, ICurrentActor actor)
{
    private IQueryable<DocumentTypesResponse> Read(bool includeDeleted = false) => db.CDocumentTypes.AsNoTracking().Where(e => includeDeleted || e.DeletedAt == null).OrderBy(e => e.DisplayOrder).ThenBy(e => e.Id).Select(e => new DocumentTypesResponse(e.Id, e.Code, e.Name, e.Description, e.TemplateId, e.Template == null ? null : e.Template.Name, e.IsActive, e.IsVisible, e.DisplayOrder, e.Notes, e.CreatedAt, e.UpdatedAt, e.DeletedAt));
    public async Task<PageResponse<DocumentTypesResponse>> ListAsync(PageQuery query, CancellationToken ct)
    {
        var rows = Read(query.IncludeDeleted);
        if (!string.IsNullOrWhiteSpace(query.Search)) { var term = query.Search.Trim().ToLowerInvariant(); rows = rows.Where(e => e.Code.ToLower().Contains(term) || e.Name.ToLower().Contains(term)); }
        return await rows.ToPageAsync(query, ct);
    }
    public async Task<DocumentTypesResponse> GetAsync(long id, CancellationToken ct) => await Read().SingleOrDefaultAsync(e => e.Id == id, ct) ?? throw new ApiFault(404, "Registro no encontrado.");
    public Task<List<OptionResponse>> OptionsAsync(CancellationToken ct) => db.CDocumentTypes.AsNoTracking().Where(e => e.IsActive && e.IsVisible && e.DeletedAt == null).OrderBy(e => e.DisplayOrder).ThenBy(e => e.Id).Select(e => new OptionResponse(e.Id, e.Code, e.Name)).Take(200).ToListAsync(ct);
    public async Task<DocumentTypesResponse> CreateAsync(DocumentTypesCreateRequest request, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        await LockTemplateAsync(request.TemplateId, ct);
        await ValidateAsync(request, null, ct);
        var e = new CDocumentTypes { IsActive = true, IsVisible = true };
        Apply(request, e);
        RecordActions.Created(e, actor.Id);
        db.CDocumentTypes.Add(e);
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
        return await GetAsync(e.Id, ct);
    }
    public async Task<DocumentTypesResponse> UpdateAsync(long id, DocumentTypesUpdateRequest request, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        await LockTemplateAsync(request.TemplateId, ct);
        var e = await RecordActions.FindAsync<CDocumentTypes>(db, id, ct);
        await ValidateAsync(request, id, ct);
        Apply(request, e);
        RecordActions.Updated(e, actor.Id);
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
        return await GetAsync(id, ct);
    }
    private async Task LockTemplateAsync(long? id, CancellationToken ct)
    {
        if (!id.HasValue) return;
        var code = await db.CDocumentTemplates.Where(t => t.Id == id.Value).Select(t => t.Code).SingleOrDefaultAsync(ct) ?? throw new ApiFault(400, "Plantilla no encontrada.");
        await db.Database.ExecuteSqlInterpolatedAsync($"SELECT pg_advisory_xact_lock(hashtextextended({code}, 0))", ct);
    }
    private async Task ValidateAsync(DocumentTypesUpdateRequest request, long? id, CancellationToken ct)
    {
        Paging.Validate(request);
        var code = RecordActions.Code(request.Code);
        if (await db.CDocumentTypes.AnyAsync(e => e.Id != id && e.Code.ToUpper() == code, ct)) throw new ApiFault(409, "El código ya está registrado, incluso entre registros eliminados.");
        if (request.TemplateId.HasValue && !await db.CDocumentTemplates.AnyAsync(t => t.Id == request.TemplateId && t.IsCurrent && t.IsActive && t.DeletedAt == null, ct)) throw new ApiFault(400, "La plantilla debe existir y ser una versión vigente y activa.");
    }
    private static void Apply(DocumentTypesUpdateRequest request, CDocumentTypes e)
    {
        e.Code = RecordActions.Code(request.Code);
        e.Name = request.Name.Trim();
        e.Description = RecordActions.Clean(request.Description);
        e.DisplayOrder = request.DisplayOrder;
        e.Notes = RecordActions.Clean(request.Notes);
        e.TemplateId = request.TemplateId;
    }
    public async Task ActiveAsync(long id, bool active, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(ct);
        var e = await RecordActions.FindAsync<CDocumentTypes>(db, id, ct);
        await LockTemplateAsync(e.TemplateId, ct);
        await db.Entry(e).ReloadAsync(ct);
        if (e.DeletedAt != null) throw new ApiFault(404, "Tipo eliminado.");
        if (active && e.TemplateId.HasValue && !await db.CDocumentTemplates.AnyAsync(t => t.Id == e.TemplateId.Value && t.IsCurrent && t.IsActive && t.DeletedAt == null, ct)) throw new ApiFault(409, "Asigna una plantilla activa y vigente antes de activar el tipo.");
        RecordActions.SetActive(e, active, actor.Id);
        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }
    public async Task VisibilityAsync(long id, bool visible, CancellationToken ct) { var e = await RecordActions.FindAsync<CDocumentTypes>(db, id, ct); RecordActions.SetVisible(e, visible, actor.Id); await db.SaveChangesAsync(ct); }
    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var e = await RecordActions.FindAsync<CDocumentTypes>(db, id, ct);
        if (await db.Documents.AnyAsync(d => d.DocumentTypeId == id && d.DeletedAt == null, ct) || await db.CallsDocumentRequirements.AnyAsync(r => r.DocumentTypeId == id && r.DeletedAt == null, ct)) throw new ApiFault(409, "El catálogo está relacionado con registros vigentes. Desactívalo en lugar de eliminarlo.");
        RecordActions.Delete(e, actor.Id);
        await db.SaveChangesAsync(ct);
    }
    public async Task RestoreAsync(long id, CancellationToken ct) { var e = await RecordActions.FindAsync<CDocumentTypes>(db, id, ct, true); if (e.DeletedAt == null) return; RecordActions.Restore(e, actor.Id); await db.SaveChangesAsync(ct); }
}
