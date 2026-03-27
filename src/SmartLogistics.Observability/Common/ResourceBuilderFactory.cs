using OpenTelemetry.Resources;

namespace SmartLogistics.Observability.Common
{
    public static class ResourceBuilderFactory
    {
        public static ResourceBuilder Create(string serviceName)
        {
            return ResourceBuilder
                .CreateDefault()
                .AddService(serviceName);
        }
    }
}
