using System.Diagnostics;

public sealed class CorrelationDelegatingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var correlationId =
            Activity.Current?.GetBaggageItem("correlation.id");

        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            request.Headers.TryAddWithoutValidation(
                "X-Correlation-Id",
                correlationId);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
