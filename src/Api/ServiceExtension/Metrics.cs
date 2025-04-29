using Api.Dev;

namespace Api.ServiceExtension;
public static class MetricsCollectionsExtensions {
  public static IServiceCollection AddMetrics(this IServiceCollection services) {
    services.AddSingleton<NorthiwndMetrics>();
    return services;
  }
}
