using System.Diagnostics;

public sealed class CorrelationMiddleware
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ILogger<CorrelationMiddleware> logger)
    {
        var correlationId =
            context.Request.Headers.TryGetValue(HeaderName, out var value)
                ? value.ToString()
                : Guid.NewGuid().ToString();

        context.Response.Headers[HeaderName] = correlationId;

        if (Activity.Current != null)
        {
            Activity.Current.SetTag("correlation.id", correlationId);
            Activity.Current.AddBaggage("correlation.id", correlationId);
        }

        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            await _next(context);
        }
    }
}
