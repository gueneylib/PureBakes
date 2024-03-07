namespace PureBakes.Service.Services;

using Microsoft.Extensions.Logging;
using PureBakes.Service.Services.Interface;

public class LogService<T>: ILogService<T> where T : class
{
    private readonly ILogger<T> _logger;

    public LogService(ILogger<T> logger)
    {
        _logger = logger;
    }
    public void LogError(Exception exception, string exMessage)
    {
        _logger.LogError(exception, exMessage);
    }
}