using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace Api.DependencyInjection;

public static class ResponseCompressionServiceCollectionExtensions {
  public static IServiceCollection AddDefaultResponseCompression(this IServiceCollection services) {
    services.AddResponseCompression(options => {
      options.Providers.Add<GzipCompressionProvider>();
      options.EnableForHttps = true;
      options.MimeTypes = ["application/json"];
    });

    services.Configure<GzipCompressionProviderOptions>(options => {
      options.Level = CompressionLevel.Fastest;
    });

    return services;
  }
}
