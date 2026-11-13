using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecNM.Research.Common;
using TecNM.Research.Data;
using TecNM.Research.Entities;

namespace TecNM.Research.Features.Calls;

[ApiController]
[Route("api/v1/calls/{callId:long:min(1)}/document-requirements")]
public class CallDocumentRequirementsController(CallDocumentRequirementsService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> List(long callId, [FromQuery] PageQuery query, CancellationToken ct) => Ok(await service.ListAsync(callId, query, ct));
    [HttpGet("{id:long:min(1)}")] public async Task<IActionResult> Get(long callId, long id, CancellationToken ct) => Ok(await service.GetAsync(callId, id, ct));
    [HttpPost] public async Task<IActionResult> Create(long callId, CallDocumentRequirementsCreateRequest request, CancellationToken ct)
    {
        var result = await service.CreateAsync(callId, request, ct);
        return CreatedAtAction(nameof(Get), new { callId, id = result.Id }, result);
    }
    [HttpPut("{id:long:min(1)}")] public async Task<IActionResult> Update(long callId, long id, CallDocumentRequirementsUpdateRequest request, CancellationToken ct) => Ok(await service.UpdateAsync(callId, id, request, ct));
    [HttpPatch("{id:long:min(1)}/active")] public async Task<IActionResult> Active(long callId, long id, ActiveRequest request, CancellationToken ct) { await service.StateAsync(callId, id, request.IsActive!.Value, null, false, false, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/visibility")] public async Task<IActionResult> Visibility(long callId, long id, VisibilityRequest request, CancellationToken ct) { await service.StateAsync(callId, id, null, request.IsVisible!.Value, false, false, ct); return NoContent(); }
    [HttpDelete("{id:long:min(1)}")] public async Task<IActionResult> Delete(long callId, long id, CancellationToken ct) { await service.StateAsync(callId, id, null, null, true, false, ct); return NoContent(); }
    [HttpPatch("{id:long:min(1)}/restore")] public async Task<IActionResult> Restore(long callId, long id, CancellationToken ct) { await service.StateAsync(callId, id, null, null, false, true, ct); return NoContent(); }
}
public class CallDocumentRequirementsUpdateRequest
{
    [Required, StringLength(300)] public string Title { get; set; } = "";
    [Range(1, long.MaxValue)] public long DocumentTypeId { get; set; }
    [Required, StringLength(40), RegularExpression("^(application|submission|academic_authorization|progress|closure)$")] public string WorkflowStage { get; set; } = "application";
    public bool IsRequired { get; set; } = true;
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}
public sealed class CallDocumentRequirementsCreateRequest : CallDocumentRequirementsUpdateRequest { }
public record CallDocumentRequirementResponse(long Id, long CallId, string Title, long DocumentTypeId, string DocumentTypeName, long? TemplateId, string? TemplateName, string WorkflowStage, bool IsRequired, bool IsActive, bool IsVisible, int DisplayOrder, string? Notes, DateTime? DeletedAt);

public class CallDocumentRequirementsService(AppDbContext db, ICurrentActor actor)
{
    public static IQueryable<CallsDocumentRequirements> Read(AppDbContext db) => db.CallsDocumentRequirements.AsNoTracking().OrderBy(e => e.DisplayOrder).ThenBy(e => e.Id);
    public static readonly Expression<Func<CallsDocumentRequirements, CallDocumentRequirementResponse>> Projection = e => new CallDocumentRequirementResponse(e.Id, e.CallId, e.Title, e.DocumentTypeId, e.DocumentType.Name, e.DocumentType.TemplateId, e.DocumentType.Template == null ? null : e.DocumentType.Template.Name, e.WorkflowStage, e.IsRequired, e.IsActive, e.IsVisible, e.DisplayOrder, e.Notes, e.DeletedAt);
    private async Task ParentAsync(long callId, CancellationToken ct)
    {
        if (!await db.CCalls.AnyAsync(c => c.Id == callId && c.DeletedAt == null, ct)) throw new ApiFault(404, "Convocatoria no encontrada.");
    }
    private async Task EditableAsync(long callId, CancellationToken ct)
    {
        await ParentAsync(callId, ct);
        if (await db.ProjectsApplications.AnyAsync(a => a.CallId == callId && a.DeletedAt == null, ct)) throw new ApiFault(409, "Los requisitos quedan congelados al recibir postulaciones; crea otra convocatoria para cambiarlos.");
    }
    public async Task<PageResponse<CallDocumentRequirementResponse>> ListAsync(long callId, PageQuery query, CancellationToken ct)
    {
        await ParentAsync(callId, ct);
        var rows = Read(db).Where(e => e.CallId == callId && (query.IncludeDeleted || e.DeletedAt == null));
        if (!string.IsNullOrWhiteSpace(query.Search)) { var term = query.Search.Trim().ToLowerInvariant(); rows = rows.Where(e => e.Title.ToLower().Contains(term)); }
        return await rows.ToPageAsync(query, Projection, ct);
    }
    public async Task<CallDocumentRequirementResponse> GetAsync(long callId, long id, CancellationToken ct)
    {
        await ParentAsync(callId, ct);
        return await Read(db).Where(e => e.CallId == callId && e.Id == id && e.DeletedAt == null).Select(Projection).SingleOrDefaultAsync(ct) ?? throw new ApiFault(404, "Requisito no encontrado en esta convocatoria.");
    }
    public async Task<CallDocumentRequirementResponse> CreateAsync(long callId, CallDocumentRequirementsCreateRequest request, CancellationToken ct)
    {
        await EditableAsync(callId, ct);
        await ValidateAsync(request, ct);
        var e = new CallsDocumentRequirements { CallId = callId, IsActive = true, IsVisible = true };
        Apply(request, e);
        RecordActions.Created(e, actor.Id);
        db.CallsDocumentRequirements.Add(e);
        await db.SaveChangesAsync(ct);
        return await GetAsync(callId, e.Id, ct);
    }
    public async Task<CallDocumentRequirementResponse> UpdateAsync(long callId, long id, CallDocumentRequirementsUpdateRequest request, CancellationToken ct)
    {
        await EditableAsync(callId, ct);
        await ValidateAsync(request, ct);
        var e = await RecordActions.FindAsync<CallsDocumentRequirements>(db, id, ct);
        if (e.CallId != callId) throw new ApiFault(404, "El requisito no pertenece a esta convocatoria.");
        Apply(request, e);
        RecordActions.Updated(e, actor.Id);
        await db.SaveChangesAsync(ct);
        return await GetAsync(callId, id, ct);
    }
    private async Task ValidateAsync(CallDocumentRequirementsUpdateRequest request, CancellationToken ct)
    {
        Paging.Validate(request);
        if (!await db.CDocumentTypes.AnyAsync(t => t.Id == request.DocumentTypeId && t.IsActive && t.DeletedAt == null, ct)) throw new ApiFault(400, "El tipo de documento debe existir y estar activo.");
        await Task.CompletedTask;
    }
    private static void Apply(CallDocumentRequirementsUpdateRequest request, CallsDocumentRequirements e)
    {
        e.Title = request.Title.Trim(); e.DocumentTypeId = request.DocumentTypeId; e.WorkflowStage = request.WorkflowStage.Trim();
        e.IsRequired = request.IsRequired; e.DisplayOrder = request.DisplayOrder; e.Notes = RecordActions.Clean(request.Notes);
    }
    public async Task StateAsync(long callId, long id, bool? active, bool? visible, bool delete, bool restore, CancellationToken ct)
    {
        await EditableAsync(callId, ct);
        var e = await RecordActions.FindAsync<CallsDocumentRequirements>(db, id, ct, restore);
        if (e.CallId != callId) throw new ApiFault(404, "El requisito no pertenece a esta convocatoria.");
        if (restore) { if (e.DeletedAt == null) return; RecordActions.Restore(e, actor.Id); }
        else if (delete) RecordActions.Delete(e, actor.Id);
        else if (active.HasValue) RecordActions.SetActive(e, active.Value, actor.Id);
        else if (visible.HasValue) RecordActions.SetVisible(e, visible.Value, actor.Id);
        await db.SaveChangesAsync(ct);
    }
}
