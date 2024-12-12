namespace Jump.LoggingSetup;

public class SerilogLogger : ILogger
{
    private readonly Serilog.ILogger _logger;

    public SerilogLogger(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        _logger.Information(message);
    }

    public void LogWarning(string message)
    {
        _logger.Warning(message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        _logger.Error(ex, message);
    }
}