using Microsoft.Extensions.Logging;

namespace Application.extensions;

public static partial class LogTemplates {
[LoggerMessage(EventId = 201, Level = LogLevel.Information, Message = "", SkipEnabledCheck = true)]
  public static partial void LogFoo(this ILogger logger);
}
