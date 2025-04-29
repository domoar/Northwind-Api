using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

namespace Api.ServiceExtension;
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
