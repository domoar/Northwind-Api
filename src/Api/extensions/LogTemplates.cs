namespace Api.extensions;

public static partial class LogTemplates {
[LoggerMessage(EventId = 101, Level = LogLevel.Information, Message = "", SkipEnabledCheck = true)]
  public static partial void LogFoo(this ILogger logger);
}
