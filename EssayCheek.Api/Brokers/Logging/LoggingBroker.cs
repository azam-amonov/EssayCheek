
namespace EssayCheek.Api.Brokers.Logging;

public class LoggingBroker : ILoggingBroker
{
    private readonly ILogger<LoggingBroker> _logger;

    public LoggingBroker(ILogger<LoggingBroker> logger) => 
        _logger = logger;

    public void LogError(Exception exception) =>
                    _logger.LogError(exception.Message, exception);

    public void LogCritical(Exception exception) =>
                    _logger.LogCritical(exception.Message, exception);
}