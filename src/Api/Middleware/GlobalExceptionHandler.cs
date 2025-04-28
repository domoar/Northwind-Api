using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler {
  private readonly ILogger<GlobalExceptionHandler> _logger;
  private readonly IProblemDetailsService _problemDetailsService;

  public GlobalExceptionHandler(
      ILogger<GlobalExceptionHandler> logger,
      IProblemDetailsService problemDetailsService) {
    _logger = logger;
    _problemDetailsService = problemDetailsService;
  }

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken ct) {

    var pd = new ProblemDetails {
      Status = StatusCodes.Status500InternalServerError,
      Title = "An unexpected error occurred.",
      Detail = exception.Message,
      Type = "server_error",
      Instance = httpContext.Request.Path
    };

    httpContext.Response.StatusCode = pd.Status!.Value;

    var handled = await _problemDetailsService.TryWriteAsync(
        new ProblemDetailsContext {
          HttpContext = httpContext,
          ProblemDetails = pd
        });

    _logger.LogError(exception, "Unhandled exception translated to ProblemDetails");

    return handled;
  }
}
