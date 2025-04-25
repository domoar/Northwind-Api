using Microsoft.OpenApi.Models;

namespace Api.ServiceExtensions;
public static class SwaggerGeneration {
  public static IServiceCollection AddDefaultSwaggerGeneration(this IServiceCollection services) {
    services.AddSwaggerGen(c => {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind Api", Version = "v1" });
    });
    return services;
  }
}
