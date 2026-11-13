using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecNM.Research.Common;
using TecNM.Research.Data;
using TecNM.Research.Infrastructure;
using DocumentEntity = TecNM.Research.Entities.Documents;

namespace TecNM.Research.Features.Documents;

[ApiController]
[Route("api/v1/documents")]
public class DocumentsController(DocumentsService service) : ControllerBase
{
    [HttpGet] public async Task<IActionResult> List([FromQuery] DocumentsQuery query, CancellationToken ct) => Ok(await service.ListAsync(query, ct));
    [HttpGet("{id:long:min(1)}")] public async Task<IActionResult> Get(long id, CancellationToken ct) => Ok(await service.GetAsync(id, ct));
    [HttpGet("{id:long:min(1)}/references")] public async Task<IActionResult> References(long id, CancellationToken ct) => Ok(await service.ReferencesAsync(id, ct));
    [HttpPost, Consumes("multipart/form-data")]
    [RequestSizeLimit(32L * 1024 * 1024)]
    public async Task<IActionResult> Upload([FromForm] DocumentUploadRequest request, CancellationToken ct)
    {
        var result = await service.UploadAsync(request, ct);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }
    [HttpGet("{id:long:min(1)}/download")]
    public async Task<IActionResult> Download(long id, CancellationToken ct)
    {
        var file = await service.DownloadAsync(id, ct);
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        return File(file.Stream, file.MediaType, file.Name);
    }
    // Contrato explícito: el trigger preserve_history prohíbe UPDATE y DELETE en files.documents.
    [HttpPut("{id:long:min(1)}")]
    public IActionResult Update(long id) => Problem(statusCode: 409, title: "El documento es inmutable. Carga otro archivo y vincúlalo como nueva versión desde su proceso propietario.");
    [HttpDelete("{id:long:min(1)}")]
    public IActionResult Delete(long id) => Problem(statusCode: 409, title: "El documento es histórico y no se elimina. Gestiona su relación con el proyecto, trámite o evidencia.");
}
public class DocumentUploadRequest
{
    [Range(1, long.MaxValue)] public long DocumentTypeId { get; set; }
    [Required] public IFormFile File { get; set; } = null!;
    [Range(0, int.MaxValue)] public int DisplayOrder { get; set; }
    public string? Notes { get; set; }
}
public class DocumentsQuery : PageQuery { [Range(1, long.MaxValue)] public long? DocumentTypeId { get; set; } }
public record DocumentResponse(long Id, long DocumentTypeId, string DocumentTypeName, string OriginalFileName, string MediaType, long SizeBytes, string Sha256, long UploadedBy, string UploadedByName, DateTime UploadedAt, bool IsActive, bool IsVisible, int DisplayOrder, string? Notes);
public record DownloadFile(FileStream Stream, string Name, string MediaType);
public record DocumentReferences(long Id, int Projects, int Applications, int Budgets, int Expenses, int Funding, int Outputs, int ExternalOutputs, int ObjectivesHistory, int MilestonesHistory, int TasksHistory, int AcademicHistory, int AcademicSubmissions);

public class DocumentsService(AppDbContext db, ICurrentActor actor, ILocalFileStorage storage)
{
    private IQueryable<DocumentEntity> Read() => db.Documents.AsNoTracking().Where(d => d.DeletedAt == null).OrderByDescending(d => d.Id);
    private static readonly Expression<Func<DocumentEntity, DocumentResponse>> Projection = d => new DocumentResponse(d.Id, d.DocumentTypeId, d.DocumentType.Name, d.OriginalFileName, d.MediaType, d.SizeBytes, d.Sha256, d.UploadedBy, d.UploadedByNavigation.GivenName + " " + d.UploadedByNavigation.FamilyName, d.UploadedAt, d.IsActive, d.IsVisible, d.DisplayOrder, d.Notes);
    public async Task<PageResponse<DocumentResponse>> ListAsync(DocumentsQuery query, CancellationToken ct)
    {
        var q = Read();
        if (query.DocumentTypeId.HasValue) q = q.Where(d => d.DocumentTypeId == query.DocumentTypeId.Value);
        if (!string.IsNullOrWhiteSpace(query.Search)) { var term = query.Search.Trim().ToLowerInvariant(); q = q.Where(d => d.OriginalFileName.ToLower().Contains(term) || d.DocumentType.Name.ToLower().Contains(term)); }
        return await q.ToPageAsync(query, Projection, ct);
    }
    public async Task<DocumentResponse> GetAsync(long id, CancellationToken ct) => await Read().Where(d => d.Id == id).Select(Projection).SingleOrDefaultAsync(ct) ?? throw new ApiFault(404, "Documento no encontrado.");
    public async Task<DocumentResponse> UploadAsync(DocumentUploadRequest request, CancellationToken ct)
    {
        Paging.Validate(request);
        var userId = actor.Id ?? throw new ApiFault(503, "Configura Development:ActorUserId para probar cargas sin login.");
        if (!await db.Users.AnyAsync(u => u.Id == userId && u.IsActive && u.DeletedAt == null, ct)) throw new ApiFault(400, "El responsable de desarrollo debe corresponder a un usuario activo existente.");
        if (!await db.CDocumentTypes.AnyAsync(t => t.Id == request.DocumentTypeId && t.IsActive && t.DeletedAt == null, ct)) throw new ApiFault(400, "El tipo de documento debe existir y estar activo.");
        var file = await storage.SaveAsync(request.File, ct);
        var d = new DocumentEntity { DocumentTypeId = request.DocumentTypeId, OriginalFileName = file.OriginalName, StorageKey = file.Key, MediaType = file.MediaType, SizeBytes = file.Size, Sha256 = file.Sha256, UploadedBy = userId, UploadedAt = DateTime.UtcNow, IsActive = true, IsVisible = true, DisplayOrder = request.DisplayOrder, Notes = RecordActions.Clean(request.Notes) };
        RecordActions.Created(d, userId);
        db.Documents.Add(d);
        // Si falla la BD no se borra el binario: una confirmación de commit podría ser ambigua.
        await db.SaveChangesAsync(ct);
        return await GetAsync(d.Id, ct);
    }
    public async Task<DownloadFile> DownloadAsync(long id, CancellationToken ct)
    {
        var d = await db.Documents.AsNoTracking().SingleOrDefaultAsync(d => d.Id == id && d.DeletedAt == null, ct) ?? throw new ApiFault(404, "Documento no encontrado.");
        return new(storage.Open(d.StorageKey), d.OriginalFileName, d.MediaType);
    }
    public async Task<DocumentReferences> ReferencesAsync(long id, CancellationToken ct)
    {
        return await db.Documents.AsNoTracking().Where(d => d.Id == id && d.DeletedAt == null).Select(d => new DocumentReferences(d.Id, d.ProjectsDocuments.Count, d.ProjectsApplicationsDocuments.Count, d.ProjectsBudgetDocuments.Count, d.ProjectsExpensesDocuments.Count, d.ProjectsFundingDocuments.Count, d.ResearchOutputsDocuments.Count, d.ExternalResearchOutputsDocuments.Count, d.ObjectivesStatusHistoryDocuments.Count, d.MilestonesStatusHistoryDocuments.Count, d.TasksStatusHistoryDocuments.Count, d.AcademicAuthorizationsHistory.Count, d.ProjectsSubmissions.Count)).SingleOrDefaultAsync(ct) ?? throw new ApiFault(404, "Documento no encontrado.");
    }
}
