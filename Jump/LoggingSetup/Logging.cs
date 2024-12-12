using Serilog;

namespace Jump.LoggingSetup;

/// <summary>
/// Class <c>Logging</c> is used to configure the logging.
/// The logger can be accessed through the <c>Logger</c> property.
/// It's fully customizable built on the Serilog package
/// </summary>
public static class Logging
{

    public static LoggingLevel LoggingLevel { get; set; } = LoggingLevel.DEFAULT;
    
    static Logging()
    {
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
        Logger = new SerilogLogger(loggerConfig.CreateLogger());
    }

    public static ILogger Logger { get; private set; }

    /// <summary>
    /// The ConfigureLogger method is used to configure the logger.
    /// This will overwrite the default logger.
    /// </summary>
    /// <param name="configure">The LoggerConfiguration to use</param>
    public static void ConfigureLogger(Action<LoggerConfiguration> configure)
    {
        var loggerConfig = new LoggerConfiguration();
        configure(loggerConfig);
        Logger = new SerilogLogger(loggerConfig.CreateLogger());
    }
}