namespace SmartLogistics.Observability.Tracing
{
    public sealed class TracingOptions
    {
        public const string SectionName = "Observability";

        public string ServiceName { get; init; } = "smart-logistics-unknown";
    }
}
