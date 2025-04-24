using Api.dev;

namespace Api.serviceExtensions;
public static class MetricsCollectionsExtensions {
  public static IServiceCollection AddMetrics(this IServiceCollection services) {
    services.AddSingleton<NorthiwndMetrics>();
    return services;
  }
}