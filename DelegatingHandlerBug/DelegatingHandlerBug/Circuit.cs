using Microsoft.AspNetCore.Components.Server.Circuits;

namespace DelegatingHandlerBug;

public class CircuitServicesAccessor
{
    static readonly AsyncLocal<IServiceProvider> blazorServices = new();

    public IServiceProvider? Services
    {
        get => blazorServices.Value;
        set => blazorServices.Value = value;
    }
}

public class ServicesAccessorCircuitHandler(
    IServiceProvider services, CircuitServicesAccessor servicesAccessor) 
    : CircuitHandler
{
    public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
        Func<CircuitInboundActivityContext, Task> next) => 
        async context =>
        {
            servicesAccessor.Services = services;
            await next(context);
            servicesAccessor.Services = null;
        };
}

public static class CircuitServicesServiceCollectionExtensions
{
    public static IServiceCollection AddCircuitServicesAccessor(
        this IServiceCollection services)
    {
        services.AddScoped<CircuitServicesAccessor>();
        services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();

        return services;
    }
}