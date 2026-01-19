namespace SmartLogistics.Api.Middleware;

public sealed class CorrelationMiddleware
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(HeaderName, out var cid))
        {
            cid = Guid.NewGuid().ToString();
            context.Request.Headers[HeaderName] = cid;
        }

        context.Response.Headers[HeaderName] = cid!;
        await _next(context);
    }
}
