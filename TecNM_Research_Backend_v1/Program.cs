using Microsoft.EntityFrameworkCore;
using TecNM.Research.Data;
using TecNM.Research.Common;
using TecNM.Research.Features.Calls;
using TecNM.Research.Features.Documents;



var builder = WebApplication.CreateBuilder(args);

// Register the Controllers (JSON: camelCase)
builder.Services.AddControllers();

builder.Services.AddResearchSharedServices();
builder.Services.AddCallsModule();
builder.Services.AddDocumentsModule();

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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Falta configurar ConnectionStrings:DefaultConnection.");
}
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));



// Application Settings

var app = builder.Build();
app.UseExceptionHandler();
app.UseCors("AllowAll");
app.MapControllers();

// Swagger exclusively in development.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Lifetime.ApplicationStarted.Register(() =>
{
    var addresses = app.Services.GetService<Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>()?.Addresses;
    var primaryUrl = addresses?.FirstOrDefault() ?? "http://localhost:5185";

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n==================================================");
    Console.WriteLine(" 🚀 TecNM Research Backend V1 (REST API) en ejecución");
    Console.WriteLine($" 📌 Servidor API iniciado en: {primaryUrl}");
    Console.WriteLine($" 🔗 Base Endpoint:             {primaryUrl}/api/v1");
    Console.WriteLine("==================================================\n");
    Console.ResetColor();
});

app.Run();

