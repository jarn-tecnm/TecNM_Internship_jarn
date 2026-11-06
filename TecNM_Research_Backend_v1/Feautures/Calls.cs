//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecNM.Research.Data;
using TecNM.Research.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace TecNM.Research.Features.Calls;

[Route("api/Calls")]
///[Authorize]
public class CallsController : Controller
{

    private readonly ICallsService _CallsService;

    public CallsController(ICallsService CallsService)
    {
        _CallsService = CallsService;
    }

    [HttpGet]
    public async Task<ActionResult<IList<CCalls>>> GetCalls()
    {
        var resultado = await _CallsService.GetCallssAsync();
        return Ok(resultado);
    }


 }

public class CallsRequest
{
    [Required(ErrorMessage = "El número de notaría es requerido")]
    [Range(1, 9999, ErrorMessage = "El número de notaría debe ser entre 1 y 9999")]
    public int NumeroCalls { get; set; }

    [MaxLength(200)]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "La calle es requerida")]
    [MaxLength(200)]
    public string Calle { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número es requerido")]
    [MaxLength(10)]
    public string NumeroExterior { get; set; } = string.Empty;

    [MaxLength(10)]
    public string NumeroInterior { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Colonia { get; set; }  // Si no está en catálogo, texto libre

    [MaxLength(10)]
    public string? CodigoPostal { get; set; }

    public int? MunicipioId { get; set; }

    public int? EstadoId { get; set; }

    [MaxLength(300)]
    public string? Observaciones { get; set; }

    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
}

public class CallsResponse
{
    public int Id { get; set; }
    public int NumeroCalls { get; set; }
    public string? Nombre { get; set; }
    public string DireccionCompleta { get; set; } = string.Empty;
    public string Calle { get; set; } = string.Empty;
    public string NumeroExterior { get; set; } = string.Empty;
    public string NumeroInterior { get; set; } = string.Empty;
    public string? Colonia { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Municipio { get; set; }
    public int? MunicipioId { get; set; }
    public string? Localidad { get; set; }
    public string? Estado { get; set; }
    public int? EstadoId { get; set; }
    public string? Observaciones { get; set; }
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string? NombreEncargado { get; set; }
    public int TotalUsuarios { get; set; }
}

public interface ICallsService
{
    Task<IList<CCalls>> GetCallssAsync();

    // Task<CallsResponse?> ObtenerPorIdAsync(int id);

    // Task<(bool Exitoso, string Mensaje, CallsResponse? Datos)> CrearAsync(CallsRequest request, int usuarioCreadorId, string ip);

    // Task<(bool Exitoso, string Mensaje)> ActualizarAsync(int id, CallsRequest request, int usuarioId, string ip);

    // Task<(bool Exitoso, string Mensaje)> ActivarDesactivarAsync(int id, int usuarioId, string ip);

}

public class CallsService : ICallsService
{

    private readonly AppDbContext _db;
    //private readonly IAuditoriaService _auditoria;
    private readonly ILogger<CallsService> _logger;

    public CallsService(AppDbContext db, ILogger<CallsService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IList<CCalls>> GetCallssAsync()
    {
        try
        {
            var result = await _db.CCalls.ToListAsync();

            return result;

           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en CallsService.GetCallssAsync");
            throw;
        }
    }

    // public async Task<CallsResponse?> ObtenerPorIdAsync(int id)
    // {
    //     try
    //     {
    //         var n = await _db.Callss
    //             .Include(n => n.Municipio)
    //                 .ThenInclude(m => m!.Estado)
    //             .Include(n => n.Usuarios.Where(u => u.Activo))
    //             .AsNoTracking()
    //             .FirstOrDefaultAsync(n => n.Id == id);
    //         return n == null ? null : MapearCalls(n);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error en CallsService.ObtenerPorIdAsync");
    //         throw;
    //     }
    // }

    // public async Task<(bool Exitoso, string Mensaje, CallsResponse? Datos)> CrearAsync(CallsRequest request, int usuarioCreadorId, string ip)
    // {
    //     try
    //     {
    //         var existente = await _db.Callss.FirstOrDefaultAsync(n => n.NumeroCalls == request.NumeroCalls);

    //         if (existente != null)
    //         {
    //             if (existente.Activo)
    //                 return (false, $"La Notaría número {request.NumeroCalls} ya existe.", null);
    //             else
    //                 return (false, $"La Notaría {request.NumeroCalls} existe pero está inactiva. " +
    //                                "¿Deseas reactivarla?", null);
    //         }

    //         var Calls = new Calls
    //         {
    //             NumeroCalls = request.NumeroCalls,
    //             Nombre = request.Nombre,
    //             Calle = request.Calle,
    //             NumeroExterior = request.NumeroExterior,
    //             NumeroInterior = request.NumeroInterior,
    //             Colonia = request.Colonia,
    //             CodigoPostal = request.CodigoPostal,
    //             MunicipioId = request.MunicipioId,
    //             // EstadoId = request.EstadoId,
    //             Observaciones = request.Observaciones,
    //             Latitud = request.Latitud,
    //             Longitud = request.Longitud,
    //             Activo = true
    //         };

    //         _db.Callss.Add(Calls);
    //         await _db.SaveChangesAsync();

    //         await _auditoria.RegistrarAsync(
    //             tabla: "Callss", registroId: Calls.Id.ToString(),
    //             accion: "INSERT", usuarioId: usuarioCreadorId,
    //             datosAnteriores: null,
    //             datosNuevos: JsonSerializer.Serialize(new {
    //                 Calls.NumeroCalls,
    //                 Calls.Nombre,
    //                 Calls.Calle,
    //                 Calls.NumeroExterior,
    //                 Calls.NumeroInterior,
    //                 Calls.Colonia,
    //                 Calls.CodigoPostal,
    //                 Calls.MunicipioId,
    //                 Calls.Observaciones,
    //                 Calls.Latitud,
    //                 Calls.Longitud,
    //                 Calls.Activo
    //                  }),
    //             ip: ip
    //         );

    //         var response = MapearCalls(Calls);
    //         return (true, "Notaría creada exitosamente", response);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error en CallsService.CrearAsync");
    //         throw;
    //     }
    // }

    // public async Task<(bool Exitoso, string Mensaje)> ActualizarAsync(int id, CallsRequest request, int usuarioId, string ip)
    // {
    //     try
    //     {
    //         var Calls = await _db.Callss.FindAsync(id);
    //         if (Calls == null) return (false, "Notaría no encontrada");

    //         var datosAnteriores = JsonSerializer.Serialize(Calls);

    //         Calls.Nombre = request.Nombre;
    //         Calls.Calle = request.Calle;
    //         Calls.NumeroExterior = request.NumeroExterior;
    //         Calls.NumeroInterior = request.NumeroInterior;
    //         Calls.Colonia = request.Colonia;
    //         Calls.CodigoPostal = request.CodigoPostal;
    //         Calls.MunicipioId = request.MunicipioId;
    //         // Calls.EstadoId = request.EstadoId;
    //         Calls.Observaciones = request.Observaciones;
    //         Calls.Latitud = request.Latitud;
    //         Calls.Longitud = request.Longitud;

    //         await _db.SaveChangesAsync();

    //         await _auditoria.RegistrarAsync(
    //             tabla: "Callss", registroId: id.ToString(),
    //             accion: "UPDATE", usuarioId: usuarioId,
    //             datosAnteriores: datosAnteriores,
    //             datosNuevos: JsonSerializer.Serialize(Calls),
    //             ip: ip
    //         );

    //         return (true, "Notaría actualizada exitosamente");
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error en CallsService.ActualizarAsync");
    //         throw;
    //     }
    // }

    // public async Task<(bool Exitoso, string Mensaje)> ActivarDesactivarAsync(int id, int usuarioId, string ip)
    // {
    //     try
    //     {
    //         var Calls = await _db.Callss.FindAsync(id);
    //         if (Calls == null) return (false, "Notaría no encontrada");

    //         var datosAnteriores = JsonSerializer.Serialize(new { Calls.Activo });

    //         Calls.Activo = !Calls.Activo;
    //         await _db.SaveChangesAsync();

    //         await _auditoria.RegistrarAsync(
    //             tabla: "Callss",
    //             registroId: id.ToString(),
    //             accion:  Calls.Activo ? "ACTIVATE" : "DEACTIVATE",
    //             usuarioId: usuarioId,
    //             datosAnteriores: datosAnteriores,
    //             datosNuevos: JsonSerializer.Serialize(new { Calls.Activo }),
    //             ip: ip
    //         );

    //         return (true, "Notaría actualizada exitosamente");
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error en CallsService.ActivarDesactivarAsync");
    //         throw;
    //     }
    // }

    // private static CallsResponse MapearCalls(Calls n) => new()
    // {
    //     Id = n.Id,
    //     NumeroCalls = n.NumeroCalls,
    //     Nombre = n.Nombre,
    //     Calle = n.Calle,
    //     NumeroExterior = n.NumeroExterior,
    //     NumeroInterior = n.NumeroInterior ?? string.Empty,
    //     Colonia = n.Colonia,
    //     CodigoPostal = n.CodigoPostal,
    //     Municipio = n.Municipio?.Nombre,
    //     MunicipioId = n.MunicipioId,
    //     Estado = n.Municipio?.Estado?.Nombre,
    //     EstadoId = n.Municipio?.EstadoId,
    //     Observaciones = n.Observaciones,
    //     Latitud = n.Latitud,
    //     Longitud = n.Longitud,
    //     Activo = n.Activo,
    //     FechaCreacion = n.FechaCreacion,
    //     DireccionCompleta = $"{n.Calle} {n.NumeroExterior}, {n.Colonia}, {n.Municipio?.Nombre}",
    //     TotalUsuarios = n.Usuarios?.Count ?? 0
    // };

}
