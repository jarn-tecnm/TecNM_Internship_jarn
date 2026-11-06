namespace TecNM.Research.Features.Documents;

public static class DocumentsRegistration
{
    public static IServiceCollection AddDocumentsModule(this IServiceCollection services)
    {
        services.AddScoped<DocumentTypesService>();
        services.AddScoped<DocumentsService>();
        services.AddScoped<TemplatesService>();
        return services;
    }
}