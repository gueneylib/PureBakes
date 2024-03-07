namespace PureBakes.Service.Services.Interface;

public interface ILogService<out T> where T : class
{
    void LogError(Exception exception, string exMessage);
}