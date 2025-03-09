using Serilog;

namespace Jump.LoggingSetup;

/// <summary>
/// Class <c>Logging</c> is used to configure the logging.
/// The logger can be accessed through the <c>Logger</c> property.
/// It's fully customizable built on the Serilog package
/// </summary>
public static class Logging
{

    public static LoggingLevel LoggingLevel { get; set; } = LoggingLevel.Default;
    
    static Logging()
    {
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
        Logger = new SerilogLogger(loggerConfig.CreateLogger());
    }

    private static ILogger Logger { get; set; }

    /// <summary>
    /// Used to set the logger if you have a custom implementation.
    /// This *should* not break anything, but it might.
    /// </summary>
    /// <param name="logger">Your custom implementation.</param>
    public static void SetLogger(ILogger logger) => Logger = logger;
    
    /// <summary>
    /// Used to log information.
    /// Logging level is Default
    /// </summary>
    /// <param name="message">Your log</param>
    public static void LogInformation(string message)
    {
        if (ShouldLog(LoggingLevel.Default)) Logger.LogInformation(message);
    }

    /// <summary>
    /// Used to log a warning.
    /// Logging level is Default.
    /// </summary>
    /// <param name="message">Your log</param>
    public static void LogWarning(string message)
    {
        if (ShouldLog(LoggingLevel.Default)) Logger.LogWarning(message);
    }
    
    /// <summary>
    /// Used to log an error.
    /// Logging level is Default
    /// </summary>
    /// <param name="message">Your log</param>
    /// <param name="ex">An exception, optional</param>
    public static void LogError(string message, Exception? ex = null)
    {
        if (ShouldLog(LoggingLevel.Default)) Logger.LogError(message, ex);
    }

    /// <summary>
    /// Used to log debug information.
    /// Logging level is Debug
    /// </summary>
    /// <param name="message">Your log</param>
    public static void LogDebug(string message)
    {
        if (ShouldLog(LoggingLevel.Debug)) Logger.LogInformation(message);
    }
    
    private static bool ShouldLog(LoggingLevel level)
    {
        return level <= LoggingLevel;
    }
    
    /// <summary>
    /// The ConfigureLogger method is used to configure the logger.
    /// This will overwrite the default logger.
    /// This wil NOT overwrite the Logging level, you still have to set this yourself.
    /// </summary>
    /// <param name="configure">The LoggerConfiguration to use</param>
    public static void ConfigureLogger(Action<LoggerConfiguration> configure)
    {
        var loggerConfig = new LoggerConfiguration();
        configure(loggerConfig);
        Logger = new SerilogLogger(loggerConfig.CreateLogger());
    }
}