using Jump;
using Jump.LoggingSetup;

Logging.LoggingLevel = LoggingLevel.DEBUG;
await JumpApplication.Run(typeof(Program));