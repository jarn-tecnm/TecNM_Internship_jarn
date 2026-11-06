namespace TecNM.Research.Features.Calls;

public static class CallsRegistration
{
    public static IServiceCollection AddCallsModule(this IServiceCollection services)
    {
        services.AddScoped<ICallsService, CallsService>();
        services.AddScoped<CallTypesService>();
        services.AddScoped<CallRequirementsService>();
        services.AddScoped<CallDocumentRequirementsService>();
        return services;
    }
}