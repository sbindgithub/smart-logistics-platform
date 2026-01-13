using Microsoft.AspNetCore.Http;
using SmartLogistics.Application;

namespace SmartLogistics.Infrastructure.Observability;

public sealed class HttpCorrelationContext : ICorrelationContext
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCorrelationContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string CorrelationId =>
        _httpContextAccessor.HttpContext?.Items[HeaderName]?.ToString()
        ?? "unknown";
}
