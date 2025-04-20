using Microsoft.Extensions.Logging;

namespace Infrastructure.extensions;

public static partial class LogTemplates {
[LoggerMessage(EventId = 401, Level = LogLevel.Information, Message = "", SkipEnabledCheck = true)]
  public static partial void LogFoo(this ILogger logger);
}
