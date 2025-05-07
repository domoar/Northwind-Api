using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Api.Extensions;

internal static class PagedResponseUtil {
  internal static string GetBaseUrl(this HttpRequest? request) {
    if (request == null) {
      return string.Empty;
    }
    return $"{request.Scheme}://{request.Host}{request.Path}";
  }

  internal static Uri BuildPagedUrl(this string baseUrl, int pageNumber, int pageSize) {
    var uriBuilder = new UriBuilder(baseUrl) {
      Query = $"pageNumber={pageNumber}&pageSize={pageSize}",
    };
    return uriBuilder.Uri;
  }

  //TODO wierd extension (FIX?)
  internal static int CalculateTotalPages(this int totalRecords, int pageSize) {
    return (int)Math.Ceiling((double)totalRecords / pageSize);
  }
}
