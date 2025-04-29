using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Api.ServiceExtension;

[Flags]
public enum TelemetryExporters {
  None = 0,
  Jaeger = 1,
  Zipkin = 2
}

public static class OpenTelemetryCollectionExtensions {
  public static IServiceCollection AddOpenTelemetryWithExporters(
       this IServiceCollection services,
       IConfiguration configuration,
       TelemetryExporters exporters) {
    var serviceName = configuration.GetValue<string>("OpenTelemetry:ServiceName") ?? "DefaultApi";

    services.AddOpenTelemetry()
        .WithTracing(tracer => {
          tracer
                  .SetSampler<AlwaysOnSampler>()
                  .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                  .AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation();

          if (exporters.HasFlag(TelemetryExporters.Jaeger))
            tracer.AddJaeger(configuration);
          if (exporters.HasFlag(TelemetryExporters.Zipkin))
            tracer.AddZipkin(configuration);
        })
        .WithMetrics(metrics => {
          metrics
                  .AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddRuntimeInstrumentation()
                  .AddMeter(
                      "Microsoft.AspNetCore.Hosting",
                      "Microsoft.AspNetCore.Server.Kestrel",
                      "Microsoft.AspNetCore.Http.Connections",
                      "System.Net.Http",
                      serviceName,
                      "Microsoft.AspNetCore.Routing",
                      "Microsoft.AspNetCore.Diagnostics",
                      "Microsoft.AspNetCore.RateLimiting");
        });

    services.Configure<OpenTelemetryLoggerOptions>(logging =>
        logging.AddOtlpExporter());

    return services;
  }

  public static TracerProviderBuilder AddJaeger(this TracerProviderBuilder builder, IConfiguration configuration) {
    var port = configuration.GetValue("Ports:JAEGER_PORT", "4317");
    builder.AddOtlpExporter(opt => {
      opt.Endpoint = new Uri($"http://jaeger:{port}");
    });
    return builder;
  }

  public static TracerProviderBuilder AddZipkin(this TracerProviderBuilder builder, IConfiguration configuration) {
    var port = configuration.GetValue("Ports:ZIPKIN_PORT", "9411");
    builder.AddZipkinExporter(opt => {
      opt.Endpoint = new Uri($"http://zipkin:{port}/api/v2/spans");
    });
    return builder;
  }
}
