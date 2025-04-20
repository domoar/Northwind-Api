using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjectionContainer {
  public static IServiceCollection AddApplication(this IServiceCollection services) {
    return services;
  }
}
