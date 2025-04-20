using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.middleware;

[Serializable]
public class ProblemException(string error, string message) : Exception(message) {
  public string Error { get; } = error;
  public new string Message { get; } = message;
}

public class ProblemExceptionHandler : IExceptionHandler {
  private readonly IProblemDetailsService _problemDetailsService;

  public ProblemExceptionHandler(IProblemDetailsService problemDetailsService) {
    _problemDetailsService = problemDetailsService;
  }

  //TODO add additional metadata if required
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken) {
    if (exception is not ProblemException problemException) {
      return true;
    }

    var problemDetails = new ProblemDetails() {
      Status = StatusCodes.Status400BadRequest,
      Title = problemException.Error,
      Detail = problemException.Message,
      Type = "Bad Request",
    };

    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

    return await _problemDetailsService.TryWriteAsync(
      new ProblemDetailsContext { HttpContext = httpContext, ProblemDetails = problemDetails }
    );
  }
}
