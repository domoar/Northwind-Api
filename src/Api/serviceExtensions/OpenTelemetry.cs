using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;


namespace Api.ServiceExtensions;
public static class OpenTelemetryCollectionExtensions {
  public static IServiceCollection AddJaeger(this IServiceCollection services, IConfiguration configuration) {
    var port = configuration.GetValue("Ports:JAEGER_PORT", "4137");
    services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
    services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
    services.Configure<OpenTelemetryLoggerOptions>(loggger => loggger.AddOtlpExporter());
    //TODO actual implementation
    return services;
  }

  public static IServiceCollection AddZipkin(this IServiceCollection services, IConfiguration configuration) {
    var port = configuration.GetValue("Ports:ZIPKIN_PORT", "4137");
    services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
    services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
    services.Configure<OpenTelemetryLoggerOptions>(loggger => loggger.AddOtlpExporter());
    //TODO actual implementation check if double service.Configure call is acceptable
    return services;
  }
}
