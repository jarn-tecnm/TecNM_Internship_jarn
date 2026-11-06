using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TecNM.Research.Data;
using TecNM.Research.Features.Calls;


var builder = WebApplication.CreateBuilder(args);

// Register the Controllers (JSON: camelCase)
builder.Services.AddControllers();

// Respuestas estándar para errores inesperados.
builder.Services.AddProblemDetails();

// Setting size limits for file uploads (30MB buffer for formats and documents)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 30 * 1024 * 1024;
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 30 * 1024 * 1024;
    options.ValueLengthLimit = 30 * 1024 * 1024;
});

if(builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// PostgreSQL Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));


// Feature Services
builder.Services.AddScoped<ICallsService, CallsService>();

// Application Settings

var app = builder.Build();

// Centralize unexpected errors.
app.UseExceptionHandler();

// Swagger exclusively in development.

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Lifetime.ApplicationStarted.Register(() =>
{
    var addresses = app.Services.GetService<Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>()?.Addresses;
    var primaryUrl = addresses?.FirstOrDefault() ?? "http://localhost:5185";

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n==================================================");
    Console.WriteLine(" 🚀 RTecNM V2 BACKEND (REST API) EN EJECUCIÓN");
    Console.WriteLine($" 📌 Servidor API iniciado en: {primaryUrl}");
    Console.WriteLine($" 🔗 Base Endpoint:             {primaryUrl}/api/v1");
    Console.WriteLine("==================================================\n");
    Console.ResetColor();
});

app.Run();

