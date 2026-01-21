using System.Diagnostics;

namespace SmartLogistics.API.Middleware;

public sealed class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(
        RequestDelegate next,
        ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await _next(context);
        sw.Stop();

        _logger.LogInformation(
            "Path={Path} StatusCode={StatusCode} DurationMs={DurationMs}",
            context.Request.Path,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds);
    }
}
