using System.Diagnostics;

namespace SmartLogistics.Observability.Tracing;

public static class ActivityExtensions
{
    public static void AddCorrelation(this Activity? activity, string correlationId)
    {
        activity?.SetTag("correlation.id", correlationId);
    }

    public static void AddBusinessKey(this Activity? activity, string key, string value)
    {
        activity?.SetTag($"business.{key}", value);
    }
}
