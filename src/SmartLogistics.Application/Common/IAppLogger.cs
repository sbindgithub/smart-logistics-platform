// SmartLogistics.Application.Common
public interface IAppLogger<T>
{
    void Info(string message);
    void Error(Exception ex, string message);
}
