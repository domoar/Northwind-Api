namespace Api.Extensions;
public static partial class LogTemplates {
  [LoggerMessage(EventId = 101, Level = LogLevel.Information, Message = "Found {Type}: {@Results}", SkipEnabledCheck = true)]
  public static partial void LogResults(this ILogger logger, Type type, object[] results);
}
