using TecNM.Research.Infrastructure;

namespace TecNM.Research.Common;

public static class SharedServicesRegistration
{
    public static IServiceCollection AddResearchSharedServices(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<ApiFaultHandler>();
        services.AddScoped<ICurrentActor, DevelopmentActor>();
        services.AddScoped<ILocalFileStorage, LocalFileStorage>();
        return services;
    }
}