using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace SmartLogistics.Infrastructure.Persistence.Interceptors;

public sealed class DbCommandPerformanceInterceptor : DbCommandInterceptor
{
    private readonly ILogger<DbCommandPerformanceInterceptor> _logger;
    private readonly ICorrelationContext _correlationContext;

    public DbCommandPerformanceInterceptor(
        ILogger<DbCommandPerformanceInterceptor> logger,
        ICorrelationContext correlationContext)
    {
        _logger = logger;
        _correlationContext = correlationContext;
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        DbDataReader result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData);
        return await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
    }

    public override async ValueTask<int> NonQueryExecutedAsync(
        DbCommand command,
        CommandExecutedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        LogIfSlow(command, eventData);
        return await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
    }

    private void LogIfSlow(DbCommand command, CommandExecutedEventData eventData)
    {
        var elapsedMs = eventData.Duration.TotalMilliseconds;

        if (elapsedMs > 500) // threshold – adjust later
        {
            _logger.LogWarning(
                "SLOW SQL ({ElapsedMs} ms) | CorrelationId={CorrelationId} | Command={CommandText}",
                elapsedMs,
                _correlationContext.CorrelationId,
                command.CommandText
            );
        }
        else
        {
            _logger.LogInformation(
                "SQL ({ElapsedMs} ms) | CorrelationId={CorrelationId}",
                elapsedMs,
                _correlationContext.CorrelationId
            );
        }
    }
}
