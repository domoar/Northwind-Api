using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.middleware;

public class GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger) {
  private readonly RequestDelegate _next = next;
  private readonly ILogger<GlobalExceptionHandler> _logger = logger;

  public async Task InvokeAsync(HttpContext httpContext) {
    try { await _next(httpContext); }
    catch (Exception ex) {

      if (ex is ProblemException) {
        return;
      }

      _logger.LogError(ex, "An error occurred during the request processing.");
      var problemDetails = new ProblemDetails {
        Status = StatusCodes.Status500InternalServerError,
        Title = "An unexpected error occurred.",
        Detail = ex.Message,
        Instance = httpContext.Request.Path,
      };

      httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      httpContext.Response.ContentType = "application/json";
      await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }
  }
}
