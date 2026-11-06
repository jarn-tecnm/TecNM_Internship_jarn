using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TecNM.Research.Data;

namespace TecNM.Research.Common;

public sealed class ApiFault(int status, string message) : Exception(message) { public int Status { get; } = status; }

// Traduce errores esperados y conflictos de PostgreSQL sin publicar SQL ni credenciales.
public sealed class ApiFaultHandler(IProblemDetailsService problems) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception error, CancellationToken ct)
    {
        var fault = error as ApiFault;
        var pg = error as PostgresException ?? error.InnerException as PostgresException;
        if (pg is not null && pg.SqlState is PostgresErrorCodes.UniqueViolation or PostgresErrorCodes.ForeignKeyViolation) fault = new ApiFault(409, "El registro está duplicado o tiene referencias incompatibles. Actualiza los datos e inténtalo nuevamente.");
        if (pg?.SqlState == PostgresErrorCodes.CheckViolation) fault = new ApiFault(400, "Los datos no cumplen las restricciones de la base de datos.");
        if (pg?.SqlState == PostgresErrorCodes.RaiseException) fault = new ApiFault(409, "La operación contradice una regla de historial o versionado de la base de datos.");
        if (fault is null) return false;
        context.Response.StatusCode = fault.Status;
        return await problems.TryWriteAsync(new ProblemDetailsContext { HttpContext = context, ProblemDetails = new ProblemDetails { Status = fault.Status, Title = fault.Message } });
    }
}

public class PageQuery
{
    [Range(1, int.MaxValue)] public int PageNumber { get; set; } = 1;
    [Range(1, 100)] public int PageSize { get; set; } = 20;
    [StringLength(250)] public string? Search { get; set; }
    public bool IncludeDeleted { get; set; }
}

public record PageResponse<T>(List<T> Items, int TotalCount, int PageNumber, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public static class Paging
{
    public static async Task<PageResponse<T>> ToPageAsync<T>(this IQueryable<T> query, PageQuery page, CancellationToken ct)
    {
        Validate(page);
        var total = await query.CountAsync(ct);
        var offset = ((long)page.PageNumber - 1) * page.PageSize;
        var items = offset >= total ? new List<T>() : await query.Skip((int)offset).Take(page.PageSize).ToListAsync(ct);
        return new(items, total, page.PageNumber, page.PageSize);
    }
    // La consulta recibida ya debe incluir filtros y un orden estable sobre entidades.
    public static async Task<PageResponse<TResponse>> ToPageAsync<TEntity, TResponse>(this IQueryable<TEntity> query, PageQuery page, Expression<Func<TEntity, TResponse>> projection, CancellationToken ct)
    {
        Validate(page);
        var total = await query.CountAsync(ct);
        var offset = ((long)page.PageNumber - 1) * page.PageSize;
        var items = offset >= total ? new List<TResponse>() : await query.Skip((int)offset).Take(page.PageSize).Select(projection).ToListAsync(ct);
        return new(items, total, page.PageNumber, page.PageSize);
    }
    public static void Validate(object value)
    {
        var errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(value, new ValidationContext(value), errors, true)) throw new ApiFault(400, string.Join(" ", errors.Select(e => e.ErrorMessage)));
    }
}

public class ActiveRequest { [Required] public bool? IsActive { get; set; } }
public class VisibilityRequest { [Required] public bool? IsVisible { get; set; } }
public record OptionResponse(long Id, string Code, string Name);

// Contrato común implementado por clases parciales; el scaffold permanece intacto.
public interface IAuditedRecord
{
    long Id { get; set; }
    bool IsActive { get; set; }
    bool IsVisible { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
    long? CreatedBy { get; set; }
    long? UpdatedBy { get; set; }
    long? DeletedBy { get; set; }
}

public static class RecordActions
{
    public static async Task<T> FindAsync<T>(AppDbContext db, long id, CancellationToken ct, bool includeDeleted = false) where T : class, IAuditedRecord
    {
        return await db.Set<T>().SingleOrDefaultAsync(e => e.Id == id && (includeDeleted || e.DeletedAt == null), ct) ?? throw new ApiFault(404, "Registro no encontrado.");
    }
    public static void Created(IAuditedRecord entity, long? actor)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = actor;
        Updated(entity, actor);
    }
    public static void Updated(IAuditedRecord entity, long? actor)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = actor;
    }
    public static void SetActive(IAuditedRecord entity, bool active, long? actor)
    {
        entity.IsActive = active;
        Updated(entity, actor);
    }
    public static void SetVisible(IAuditedRecord entity, bool visible, long? actor)
    {
        entity.IsVisible = visible;
        Updated(entity, actor);
    }
    public static void Delete(IAuditedRecord entity, long? actor)
    {
        entity.DeletedAt = DateTime.UtcNow;
        entity.DeletedBy = actor;
        SetActive(entity, false, actor);
        SetVisible(entity, false, actor);
    }
    public static void Restore(IAuditedRecord entity, long? actor)
    {
        entity.DeletedAt = null;
        entity.DeletedBy = null;
        // Restaurar no publica ni activa automáticamente.
        SetActive(entity, false, actor);
        SetVisible(entity, false, actor);
    }
    public static string Code(string value) => value.Trim().ToUpperInvariant();
    public static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

public interface ICurrentActor { long? Id { get; } }

// Adaptador temporal: nunca se obtiene el responsable desde el cuerpo o un encabezado del cliente.
public sealed class DevelopmentActor(IWebHostEnvironment environment, IConfiguration configuration) : ICurrentActor
{
    public long? Id => environment.IsDevelopment() ? configuration.GetValue<long?>("Development:ActorUserId") : null;
}
