using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SmartLogistics.Observability.Tracing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSmartLogisticsTracing(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var serviceName =
            configuration["Observability:ServiceName"]
            ?? throw new InvalidOperationException(
                "Observability:ServiceName is missing");

        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder.SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName));
            });

        return services;
    }
}
