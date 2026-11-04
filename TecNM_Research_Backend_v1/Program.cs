using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TecNM.Research.Activities;
using TecNM.Research.Admin;
using TecNM.Research.Advisors;
using TecNM.Research.Auth;
using TecNM.Research.Common;
using TecNM.Research.Common.EmailVerification;
using TecNM.Research.Common.Notifications;
using TecNM.Research.Common.Settings;
using TecNM.Research.Companies;
using TecNM.Research.Documents;
using TecNM.Research.Evaluations;
using TecNM.Research.Careers;
using TecNM.Research.Projects;
using TecNM.Research.Notifications;
using TecNM.Research.Searches;
using TecNM.Research.Students;
using TecNM.Research.OfficialTemplates;

var builder = WebApplication.CreateBuilder(args);

// Configuración de límites de tamaño para subida de archivos (30MB de buffer para formatos y documentos)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 30 * 1024 * 1024;
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 30 * 1024 * 1024;
    options.ValueLengthLimit = 30 * 1024 * 1024;
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAdvisorRepository, AdvisorRepository>();
builder.Services.AddScoped<IAdvisorService, AdvisorService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<IDashboardMetricsService, DashboardMetricsService>();
builder.Services.AddScoped<IReportGeneratorService, ReportGeneratorService>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddSingleton<SearchRegistry>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ICareerRepository, CareerRepository>();
builder.Services.AddScoped<ICareerService, CareerService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOfficialTemplateRepository, OfficialTemplateRepository>();
builder.Services.AddScoped<IOfficialTemplateService, OfficialTemplateService>();

// Email Notification Services
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.SectionName));
builder.Services.AddSingleton<IEmailQueue, EmailQueue>();
builder.Services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<ISystemSettingService, SystemSettingService>();
builder.Services.AddHostedService<EmailBackgroundWorker>();

// Institutional Email Verification & Options
builder.Services.Configure<InstitutionalEmailOptions>(builder.Configuration.GetSection(InstitutionalEmailOptions.SectionName));
var instEmailConfig = builder.Configuration.GetSection(InstitutionalEmailOptions.SectionName).Get<InstitutionalEmailOptions>();
if (instEmailConfig?.AllowedDomains != null && instEmailConfig.AllowedDomains.Count > 0)
{
    InstitutionalEmail.AllowedDomains = instEmailConfig.AllowedDomains;
}
builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var uploadsDir = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, app.Configuration["Uploads:Path"] ?? "uploads", "documents"));
    if (!Directory.Exists(uploadsDir))
    {
        Directory.CreateDirectory(uploadsDir);
    }

    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        // Bootstrap de catálogos RBAC y admin inicial: corre SIEMPRE (producción incluida).
        var adminPassword = app.Configuration["Seed:AdminPassword"];
        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            adminPassword = "Admin2026!";
            if (!app.Environment.IsDevelopment())
                Console.WriteLine("[Seed Warning] 'Seed:AdminPassword' no configurada; usando contraseña por defecto para el admin inicial.");
        }
        await DbSeeder.BootstrapSystemAsync(dbContext, adminPassword);

        // Remediación idempotente de acreditaciones InnovaTec completadas indebidamente
        await AccreditationRemediation.RemediateAutocompletedAccreditationsAsync(dbContext, app.Logger);

        // Confirmar solicitudes para todos los estudiantes con anteproyecto activo enviado
        await SolicitudRemediation.RemediateActiveProjectSolicitudesAsync(dbContext, app.Logger);

        // Sincronizar perfil dual de Asesor para Jefes de Carrera
        await CareerHeadAdvisorRemediation.RemediateCareerHeadAdvisorsAsync(dbContext, app.Logger);

        // Generar/Asegurar plantillas Excel por defecto (Plantilla_Alumnos.xlsx y Plantilla_Empresas.xlsx)
        ExcelTemplateSeeder.EnsureTemplatesExist(app.Environment.ContentRootPath, app.Logger);

        // Datos demo (usuarios de prueba y anteproyecto con cronograma): solo desarrollo.
        var demoEnabled = app.Environment.IsDevelopment()
                          || app.Configuration.GetValue<bool>("Seed:Enabled");
        if (demoEnabled)
        {
            await DbSeeder.SeedDemoDataAsync(dbContext);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Database Warning] {ex.ToString()}");
    }
}

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

