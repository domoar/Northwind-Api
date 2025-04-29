using OpenTelemetry.Logs;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace Api.ServiceExtension;
public static class LoggingCollectionExtensions {
  public static IServiceCollection AddLoggerWithSeq(this IServiceCollection services, IConfiguration configuration) {

    Log.Logger = new LoggerConfiguration()
           .Enrich.FromLogContext()
           .MinimumLevel.Information()
           .WriteTo.Console()
           .WriteTo.OpenTelemetry(otlp => {
             otlp.Endpoint = "http://seq:80/ingest/otlp/v1/logs";
             otlp.Protocol = OtlpProtocol.HttpProtobuf;
             otlp.Headers = new Dictionary<string, string> {
               ["X-Seq-ApiKey"] = "INSERT_KEY"
             };
             otlp.ResourceAttributes = new Dictionary<string, object> {
               ["service.name"] = "Northwind-Api"
             };
           })
           .ReadFrom.Configuration(configuration)
           .CreateLogger();

    services.AddLogging(logging => {
      logging.ClearProviders();
      logging.AddSerilog(Log.Logger, dispose: true);
      logging.AddOpenTelemetry(x => x.AddOtlpExporter());
    });

    return services;
  }

  public static IServiceCollection AddDefaultLogger(this IServiceCollection services, IConfiguration configuration) {

    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

    services.AddLogging(logging => {
      logging.ClearProviders();
      logging.AddSerilog(Log.Logger, dispose: true);
    });
    return services;
  }

}
