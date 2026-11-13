using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecNM.Research.Common;
using TecNM.Research.Data;
using TecNM.Research.Entities;

namespace TecNM.Research.Features.Calls;

[ApiController]
[Route("api/v1/calls")]
public class CallsController(ICallsService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> List([FromQuery] CallsQuery query, CancellationToken ct) => Ok(await service.ListAsync(query, false, ct));
    [HttpGet("open")] public async Task<IActionResult> Open([FromQuery] CallsQuery query, CancellationToken ct) => Ok(await service.ListAsync(query, true, ct));
    [HttpGet("options")] public async Task<IActionResult> Options(CancellationToken ct) => Ok(await service.OptionsAsync(ct));
    [HttpGet("{id:long:min(1)}")] public async Task<IActionResult> Get(long id, CancellationToken ct) => Ok(await service.GetAsync(id, ct));
    [HttpPost] public async Task<IActionResult> Create(CallCreateRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    [HttpPut("{id:long:min(1)}")] public async Task<IActionResult> Update(long id, CallUpdateRequest request, CancellationToken ct) => Ok(await service.UpdateAsync(id, request, ct));
    [HttpPatch("{id:long:min(1)}/active")] public async Task<IActionResult> Active(long id, ActiveRequest request, CancellationToken ct) { await service.ActiveAsync(id, request.IsActive!.Value, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/visibility")] public async Task<IActionResult> Visibility(long id, VisibilityRequest request, CancellationToken ct) { await service.VisibilityAsync(id, request.IsVisible!.Value, ct); return NoContent(); }
    [HttpDelete("{id:long:min(1)}")] public async Task<IActionResult> Delete(long id, CancellationToken ct) { await service.DeleteAsync(id, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/restore")] public async Task<IActionResult> Restore(long id, CancellationToken ct) { await service.RestoreAsync(id, ct); return NoContent(); }
}

public class CallUpdateRequest : IValidatableObject
{
    [Required, StringLength(80)] public string Code { get; set; } = "";
    [Required, StringLength(250)] public string Name { get; set; } = "";
    public string? Description { get; set; }
    [Range(1, long.MaxValue)] public long OrganizationId { get; set; }
    [Range(1, long.MaxValue)] public long CallTypeId { get; set; }
    [Required] public DateTimeOffset? OpensAt { get; set; }
    [Required] public DateTimeOffset? ClosesAt { get; set; }
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (OpensAt.HasValue && ClosesAt.HasValue && OpensAt.Value >= ClosesAt.Value) yield return new ValidationResult("El cierre debe ser posterior a la apertura.", [nameof(ClosesAt)]);
    }
}
public sealed class CallCreateRequest : CallUpdateRequest { }
public class CallsQuery : PageQuery
{
    [Range(1, long.MaxValue)] public long? OrganizationId { get; set; }
    [Range(1, long.MaxValue)] public long? CallTypeId { get; set; }
    [Range(1, 4)] public int? StatusId { get; set; }
}
public enum CallReceptionStatus { Inactive = 1, Scheduled = 2, Open = 3, Closed = 4 }
public class CallResponse
{
    public long Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public long OrganizationId { get; set; }
    public string OrganizationName { get; set; } = "";
    public long CallTypeId { get; set; }
    public string CallTypeName { get; set; } = "";
    public DateTime OpensAt { get; set; }
    public DateTime ClosesAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsVisible { get; set; }
    public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public CallReceptionStatus StatusId { get; set; }
    public string StatusName => StatusId switch { CallReceptionStatus.Inactive => "Inactiva", CallReceptionStatus.Scheduled => "Programada", CallReceptionStatus.Open => "Abierta", CallReceptionStatus.Closed => "Cerrada", _ => "Desconocido" };
}
public record CallDetailResponse(CallResponse Call, List<CallRequirementResponse> Requirements, List<CallDocumentRequirementResponse> DocumentRequirements);

public interface ICallsService
{
    Task<PageResponse<CallResponse>> ListAsync(CallsQuery query, bool onlyOpen, CancellationToken ct);
    Task<List<OptionResponse>> OptionsAsync(CancellationToken ct);
    Task<CallDetailResponse> GetAsync(long id, CancellationToken ct);
    Task<CallResponse> CreateAsync(CallCreateRequest request, CancellationToken ct);
    Task<CallResponse> UpdateAsync(long id, CallUpdateRequest request, CancellationToken ct);
    Task ActiveAsync(long id, bool active, CancellationToken ct);
    Task VisibilityAsync(long id, bool visible, CancellationToken ct);
    Task DeleteAsync(long id, CancellationToken ct);
    Task RestoreAsync(long id, CancellationToken ct);
}

public class CallsService(AppDbContext db, ICurrentActor actor) : ICallsService
{
    private static IQueryable<CCalls> OpenCalls(IQueryable<CCalls> q, DateTime now) => q.Where(c => c.DeletedAt == null && c.IsActive && c.IsVisible && c.OpensAt <= now && now < c.ClosesAt && c.Organization.IsActive && c.Organization.IsVisible && c.Organization.DeletedAt == null && c.CallType.IsActive && c.CallType.IsVisible && c.CallType.DeletedAt == null);
    private static Expression<Func<CCalls, CallResponse>> Project(DateTime now, bool publicRead = false) => c => new CallResponse
    {
        Id = c.Id, Code = c.Code, Name = c.Name, Description = c.Description,
        OrganizationId = c.OrganizationId, OrganizationName = c.Organization.Name, CallTypeId = c.CallTypeId, CallTypeName = c.CallType.Name,
        OpensAt = c.OpensAt, ClosesAt = c.ClosesAt, IsActive = c.IsActive, IsVisible = c.IsVisible, DisplayOrder = c.DisplayOrder,
        Notes = publicRead ? null : c.Notes, CreatedAt = c.CreatedAt, UpdatedAt = c.UpdatedAt, DeletedAt = c.DeletedAt,
        StatusId = !c.IsActive ? CallReceptionStatus.Inactive : now < c.OpensAt ? CallReceptionStatus.Scheduled : now < c.ClosesAt ? CallReceptionStatus.Open : CallReceptionStatus.Closed
    };
    public async Task<PageResponse<CallResponse>> ListAsync(CallsQuery query, bool onlyOpen, CancellationToken ct)
    {
        Paging.Validate(query);
        var now = DateTime.UtcNow;
        var q = db.CCalls.AsNoTracking().Where(c => (!onlyOpen && query.IncludeDeleted) || c.DeletedAt == null);
        if (query.OrganizationId.HasValue) q = q.Where(c => c.OrganizationId == query.OrganizationId.Value);
        if (query.CallTypeId.HasValue) q = q.Where(c => c.CallTypeId == query.CallTypeId.Value);
        if (!string.IsNullOrWhiteSpace(query.Search)) { var term = query.Search.Trim().ToLowerInvariant(); q = q.Where(c => c.Code.ToLower().Contains(term) || c.Name.ToLower().Contains(term)); }
        if (onlyOpen) q = OpenCalls(q, now);
        q = query.StatusId switch
        {
            1 => q.Where(c => !c.IsActive),
            2 => q.Where(c => c.IsActive && now < c.OpensAt),
            3 => q.Where(c => c.IsActive && c.OpensAt <= now && now < c.ClosesAt),
            4 => q.Where(c => c.IsActive && c.ClosesAt <= now),
            _ => q
        };
        return await q.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToPageAsync(query, Project(now, onlyOpen), ct);
    }
    public Task<List<OptionResponse>> OptionsAsync(CancellationToken ct) => OpenCalls(db.CCalls.AsNoTracking(), DateTime.UtcNow).OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).Select(c => new OptionResponse(c.Id, c.Code, c.Name)).Take(200).ToListAsync(ct);
    public async Task<CallDetailResponse> GetAsync(long id, CancellationToken ct)
    {
        var call = await ReadOneAsync(id, ct);
        var requirements = await CallRequirementsService.Read(db).Where(r => r.CallId == id && r.DeletedAt == null).Select(CallRequirementsService.Projection).ToListAsync(ct);
        var documents = await CallDocumentRequirementsService.Read(db).Where(r => r.CallId == id && r.DeletedAt == null).Select(CallDocumentRequirementsService.Projection).ToListAsync(ct);
        return new(call, requirements, documents);
    }
    private async Task<CallResponse> ReadOneAsync(long id, CancellationToken ct) => await db.CCalls.AsNoTracking().Where(c => c.Id == id && c.DeletedAt == null).Select(Project(DateTime.UtcNow)).SingleOrDefaultAsync(ct) ?? throw new ApiFault(404, "Convocatoria no encontrada.");
    public async Task<CallResponse> CreateAsync(CallCreateRequest request, CancellationToken ct)
    {
        await ValidateAsync(request, null, ct);
        var call = new CCalls { IsActive = true, IsVisible = false };
        Apply(request, call);
        RecordActions.Created(call, actor.Id);
        db.CCalls.Add(call);
        await db.SaveChangesAsync(ct);
        return await ReadOneAsync(call.Id, ct);
    }
    public async Task<CallResponse> UpdateAsync(long id, CallUpdateRequest request, CancellationToken ct)
    {
        var call = await RecordActions.FindAsync<CCalls>(db, id, ct);
        await ValidateAsync(request, call, ct);
        if ((call.OrganizationId != request.OrganizationId || call.CallTypeId != request.CallTypeId) && await db.ProjectsApplications.AnyAsync(a => a.CallId == id && a.DeletedAt == null, ct)) throw new ApiFault(409, "No se puede cambiar organización o tipo después de recibir postulaciones.");
        Apply(request, call);
        RecordActions.Updated(call, actor.Id);
        await db.SaveChangesAsync(ct);
        return await ReadOneAsync(id, ct);
    }
    private async Task ValidateAsync(CallUpdateRequest request, CCalls? current, CancellationToken ct)
    {
        Paging.Validate(request);
        var id = current?.Id;
        var code = RecordActions.Code(request.Code);
        if (await db.CCalls.AnyAsync(c => c.Id != id && c.Code.ToUpper() == code, ct)) throw new ApiFault(409, "El código ya existe, incluso entre convocatorias eliminadas.");
        if ((current is null || current.OrganizationId != request.OrganizationId) && !await db.COrganizations.AnyAsync(o => o.Id == request.OrganizationId && o.IsActive && o.DeletedAt == null, ct)) throw new ApiFault(400, "La organización debe existir y estar activa.");
        if ((current is null || current.CallTypeId != request.CallTypeId) && !await db.CCallTypes.AnyAsync(t => t.Id == request.CallTypeId && t.IsActive && t.DeletedAt == null, ct)) throw new ApiFault(400, "El tipo de convocatoria debe existir y estar activo.");
    }
    private static void Apply(CallUpdateRequest request, CCalls call)
    {
        call.Code = RecordActions.Code(request.Code); call.Name = request.Name.Trim(); call.Description = RecordActions.Clean(request.Description);
        call.OrganizationId = request.OrganizationId; call.CallTypeId = request.CallTypeId;
        call.OpensAt = request.OpensAt!.Value.UtcDateTime; call.ClosesAt = request.ClosesAt!.Value.UtcDateTime;
        call.DisplayOrder = request.DisplayOrder; call.Notes = RecordActions.Clean(request.Notes);
    }
    public async Task ActiveAsync(long id, bool active, CancellationToken ct) { var c = await RecordActions.FindAsync<CCalls>(db, id, ct); RecordActions.SetActive(c, active, actor.Id); await db.SaveChangesAsync(ct); }
    public async Task VisibilityAsync(long id, bool visible, CancellationToken ct) { var c = await RecordActions.FindAsync<CCalls>(db, id, ct); RecordActions.SetVisible(c, visible, actor.Id); await db.SaveChangesAsync(ct); }
    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var call = await RecordActions.FindAsync<CCalls>(db, id, ct);
        if (await db.ProjectsApplications.AnyAsync(a => a.CallId == id && a.DeletedAt == null, ct)) throw new ApiFault(409, "La convocatoria tiene postulaciones. Desactívala en lugar de eliminarla.");
        RecordActions.Delete(call, actor.Id);
        await db.SaveChangesAsync(ct);
    }
    public async Task RestoreAsync(long id, CancellationToken ct) { var c = await RecordActions.FindAsync<CCalls>(db, id, ct, true); if (c.DeletedAt == null) return; RecordActions.Restore(c, actor.Id); await db.SaveChangesAsync(ct); }
}
