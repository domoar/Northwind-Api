using System;
using System.Text.Json.Serialization;

namespace Api.Response;

public class Response<T> {
  [JsonPropertyName("data")]
  public T? Data { get; set; }

  [JsonPropertyName("succeeded")]
  public bool Succeeded { get; set; }

  [JsonPropertyName("errors")]
  public string[] Errors { get; set; } = [];

  [JsonPropertyName("message")]
  public string? Message { get; set; }
}
