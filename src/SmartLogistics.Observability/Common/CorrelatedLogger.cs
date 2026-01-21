// SmartLogistics.Observability
using Microsoft.Extensions.Logging;
using SmartLogistics.Application.Common;
namespace SmartLogistics.Observability.Common;
public sealed class CorrelatedLogger<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;
    private readonly ICorrelationContext _correlationContext;

    public CorrelatedLogger(
        ILogger<T> logger,
        ICorrelationContext correlationContext)
    {
        _logger = logger;
        _correlationContext = correlationContext;
    }

    public void Error(Exception ex, string message)
    {
        _logger.LogError(
            ex,
            "CorrelationId={CorrelationId} {Message}",
            _correlationContext.CorrelationId,
            message);
    }


    public void Info(string message)
    {
        _logger.LogInformation(
            "CorrelationId={CorrelationId} {Message}",
            _correlationContext.CorrelationId,
            message);
    }
}
