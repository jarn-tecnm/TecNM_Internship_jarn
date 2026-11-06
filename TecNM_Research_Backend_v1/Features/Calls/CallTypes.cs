using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecNM.Research.Common;
using TecNM.Research.Data;
using TecNM.Research.Entities;

namespace TecNM.Research.Features.Calls;

[ApiController]
[Route("api/v1/call-types")]
public class CallTypesController(CallTypesService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> List([FromQuery] PageQuery query, CancellationToken ct) => Ok(await service.ListAsync(query, ct));
    [HttpGet("options")] public async Task<IActionResult> Options(CancellationToken ct) => Ok(await service.OptionsAsync(ct));
    [HttpGet("{id:long:min(1)}")] public async Task<IActionResult> Get(long id, CancellationToken ct) => Ok(await service.GetAsync(id, ct));
    [HttpPost] public async Task<IActionResult> Create(CallTypesCreateRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    [HttpPut("{id:long:min(1)}")] public async Task<IActionResult> Update(long id, CallTypesUpdateRequest request, CancellationToken ct) => Ok(await service.UpdateAsync(id, request, ct));
    [HttpPatch("{id:long:min(1)}/active")] public async Task<IActionResult> Active(long id, ActiveRequest request, CancellationToken ct) { await service.ActiveAsync(id, request.IsActive!.Value, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/visibility")] public async Task<IActionResult> Visibility(long id, VisibilityRequest request, CancellationToken ct) { await service.VisibilityAsync(id, request.IsVisible!.Value, ct); return NoContent(); }
    [HttpDelete("{id:long:min(1)}")] public async Task<IActionResult> Delete(long id, CancellationToken ct) { await service.DeleteAsync(id, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/restore")] public async Task<IActionResult> Restore(long id, CancellationToken ct) { await service.RestoreAsync(id, ct); return NoContent(); }
}

public class CallTypesUpdateRequest
{
    [Required, StringLength(80)] public string Code { get; set; } = "";
    [Required, StringLength(250)] public string Name { get; set; } = "";
    public string? Description { get; set; }
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
    
}
public sealed class CallTypesCreateRequest : CallTypesUpdateRequest { }
public record CallTypesResponse(long Id, string Code, string Name, string? Description, bool IsActive, bool IsVisible, int DisplayOrder, string? Notes, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);

public class CallTypesService(AppDbContext db, ICurrentActor actor)
{
    private IQueryable<CallTypesResponse> Read(bool includeDeleted = false) => db.CCallTypes.AsNoTracking().Where(e => includeDeleted || e.DeletedAt == null).OrderBy(e => e.DisplayOrder).ThenBy(e => e.Id).Select(e => new CallTypesResponse(e.Id, e.Code, e.Name, e.Description, e.IsActive, e.IsVisible, e.DisplayOrder, e.Notes, e.CreatedAt, e.UpdatedAt, e.DeletedAt));
    public async Task<PageResponse<CallTypesResponse>> ListAsync(PageQuery query, CancellationToken ct)
    {
        var rows = db.CCallTypes.AsNoTracking().Where(e => query.IncludeDeleted || e.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var term = query.Search.Trim().ToLowerInvariant();
            rows = rows.Where(e => e.Code.ToLower().Contains(term) || e.Name.ToLower().Contains(term));
        }

        var total = await rows.CountAsync(ct);
        var offset = ((long)query.PageNumber - 1) * query.PageSize;
        var items = new List<CallTypesResponse>();

        if (offset < total)
        {
            items = await rows
                .OrderBy(e => e.DisplayOrder)
                .ThenBy(e => e.Id)
                .Skip((int)offset)
                .Take(query.PageSize)
                .Select(e => new CallTypesResponse(e.Id, e.Code, e.Name, e.Description, e.IsActive, e.IsVisible, e.DisplayOrder, e.Notes, e.CreatedAt, e.UpdatedAt, e.DeletedAt))
                .ToListAsync(ct);
        }

        return new PageResponse<CallTypesResponse>(items, total, query.PageNumber, query.PageSize);
    }
    public async Task<CallTypesResponse> GetAsync(long id, CancellationToken ct)
    {
        var result = await db.CCallTypes
            .AsNoTracking()
            .Where(e => e.Id == id && e.DeletedAt == null)
            .Select(e => new CallTypesResponse(e.Id, e.Code, e.Name, e.Description, e.IsActive, e.IsVisible, e.DisplayOrder, e.Notes, e.CreatedAt, e.UpdatedAt, e.DeletedAt))
            .SingleOrDefaultAsync(ct);

        return result ?? throw new ApiFault(404, "Tipo de convocatoria no encontrado.");
    }
    public Task<List<OptionResponse>> OptionsAsync(CancellationToken ct) => db.CCallTypes.AsNoTracking().Where(e => e.IsActive && e.IsVisible && e.DeletedAt == null).OrderBy(e => e.DisplayOrder).ThenBy(e => e.Id).Select(e => new OptionResponse(e.Id, e.Code, e.Name)).Take(200).ToListAsync(ct);
    public async Task<CallTypesResponse> CreateAsync(CallTypesCreateRequest request, CancellationToken ct)
    {
        await ValidateAsync(request, null, ct);
        var e = new CCallTypes { IsActive = true, IsVisible = true };
        Apply(request, e);
        RecordActions.Created(e, actor.Id);
        db.CCallTypes.Add(e);
        await db.SaveChangesAsync(ct);
        return await GetAsync(e.Id, ct);
    }
    public async Task<CallTypesResponse> UpdateAsync(long id, CallTypesUpdateRequest request, CancellationToken ct)
    {
        var e = await RecordActions.FindAsync<CCallTypes>(db, id, ct);
        await ValidateAsync(request, id, ct);
        Apply(request, e);
        RecordActions.Updated(e, actor.Id);
        await db.SaveChangesAsync(ct);
        return await GetAsync(id, ct);
    }
    private async Task ValidateAsync(CallTypesUpdateRequest request, long? id, CancellationToken ct)
    {
        Paging.Validate(request);
        var code = RecordActions.Code(request.Code);
        if (await db.CCallTypes.AnyAsync(e => e.Id != id && e.Code.ToUpper() == code, ct)) throw new ApiFault(409, "El código ya está registrado, incluso entre registros eliminados.");
        
    }
    private static void Apply(CallTypesUpdateRequest request, CCallTypes e)
    {
        e.Code = RecordActions.Code(request.Code);
        e.Name = request.Name.Trim();
        e.Description = RecordActions.Clean(request.Description);
        e.DisplayOrder = request.DisplayOrder;
        e.Notes = RecordActions.Clean(request.Notes);
        
    }
    public async Task ActiveAsync(long id, bool active, CancellationToken ct) { var e = await RecordActions.FindAsync<CCallTypes>(db, id, ct); RecordActions.SetActive(e, active, actor.Id); await db.SaveChangesAsync(ct); }
    public async Task VisibilityAsync(long id, bool visible, CancellationToken ct) { var e = await RecordActions.FindAsync<CCallTypes>(db, id, ct); RecordActions.SetVisible(e, visible, actor.Id); await db.SaveChangesAsync(ct); }
    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var e = await RecordActions.FindAsync<CCallTypes>(db, id, ct);
        if (await db.CCalls.AnyAsync(c => c.CallTypeId == id && c.DeletedAt == null, ct)) throw new ApiFault(409, "El catálogo está relacionado con registros vigentes. Desactívalo en lugar de eliminarlo.");
        RecordActions.Delete(e, actor.Id);
        await db.SaveChangesAsync(ct);
    }
    public async Task RestoreAsync(long id, CancellationToken ct) { var e = await RecordActions.FindAsync<CCallTypes>(db, id, ct, true); if (e.DeletedAt == null) return; RecordActions.Restore(e, actor.Id); await db.SaveChangesAsync(ct); }
}
