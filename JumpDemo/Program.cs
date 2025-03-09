using Jump;
using Jump.LoggingSetup;

Logging.LoggingLevel = LoggingLevel.Debug;
await JumpApplication.Run(typeof(Program));