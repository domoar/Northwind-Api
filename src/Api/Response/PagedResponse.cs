using System;

using System.Text.Json.Serialization;
using Api.Extensions;
namespace Api.Response;

public class PagedResponse<T> : Response<T> {
  [JsonPropertyName("pageNumber")]
  public int PageNumber { get; set; }

  [JsonPropertyName("pageSize")]
  public int PageSize { get; set; }

  [JsonPropertyName("firstPage")]
  public Uri? FirstPage {
    get {
      var baseUrl = Request.GetBaseUrl();
      return new Uri($"{baseUrl}?pageNumber=1&pageSize={PageSize}");
    }
  }

  [JsonPropertyName("lastPage")]
  public Uri? LastPage {
    get {
      var baseUrl = Request.GetBaseUrl();
      return baseUrl.BuildPagedUrl(TotalPages, PageSize);
    }
  }

  [JsonPropertyName("totalPages")]
  public int TotalPages => TotalRecords.CalculateTotalPages(PageSize);

  [JsonPropertyName("totalRecords")]
  public int TotalRecords { get; set; }

  [JsonPropertyName("nextPage")]
  public Uri? NextPage {
    get {
      var baseUrl = Request.GetBaseUrl();
      return baseUrl.BuildPagedUrl(PageNumber + 1, PageSize);
    }
  }

  [JsonPropertyName("previousPage")]
  public Uri? PreviousPage {
    get {
      var baseUrl = Request.GetBaseUrl();
      return PageNumber > 1 ? baseUrl.BuildPagedUrl(PageNumber - 1, PageSize) : null;
    }
  }

  [JsonIgnore]
  public HttpRequest? Request { get; set; }
}
