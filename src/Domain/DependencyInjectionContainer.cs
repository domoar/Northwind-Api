using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjectionContainer {
  public static IServiceCollection AddDomain(this IServiceCollection services) {
    return services;
  }
}
