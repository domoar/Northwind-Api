using Microsoft.Extensions.Logging;

namespace Domain.Extensions;
public static partial class LogTemplates {
  [LoggerMessage(EventId = 301, Level = LogLevel.Information, Message = "", SkipEnabledCheck = true)]
  public static partial void LogFoo(this ILogger logger);
}
