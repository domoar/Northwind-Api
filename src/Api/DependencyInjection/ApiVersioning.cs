using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Api.DependencyInjection;

public static class ApiVersioningServiceCollectionExtensions {
  public static IServiceCollection AddDefaultApiVersioning(this IServiceCollection services) {
    services.AddApiVersioning(options => {
      options.AssumeDefaultVersionWhenUnspecified = true;
      options.DefaultApiVersion = ApiVersion.Default;
      options.ApiVersionReader = ApiVersionReader.Combine(
          new MediaTypeApiVersionReader("version"),
          new HeaderApiVersionReader("X-Version")
      );
      options.ReportApiVersions = true;
    });

    services.AddVersionedApiExplorer(setup => {
      setup.GroupNameFormat = "'v'VVV";
      setup.SubstituteApiVersionInUrl = true;
    });

    return services;
  }
}
