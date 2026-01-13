namespace SmartLogistics.Application;

public interface ICorrelationContext
{
    string CorrelationId { get; }
}
