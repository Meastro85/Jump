namespace Jump.LoggingSetup;

public sealed class SerilogLogger(Serilog.ILogger logger) : ILogger
{
    public void LogInformation(string message)
    {
        logger.Information(message);
    }

    public void LogWarning(string message)
    {
        logger.Warning(message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        logger.Error(ex, message);
    }
}