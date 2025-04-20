using Microsoft.OpenApi.Models;

namespace Api.DependencyInjection;

public static class SwaggerGeneration {
  public static IServiceCollection AddDefaultSwaggerGeneration(this IServiceCollection services) {
    services.AddSwaggerGen(c => {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Clean Architecture Template", Version = "v1" });
    });
    return services;
  }
}
