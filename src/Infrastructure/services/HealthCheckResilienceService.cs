using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.services;

public class HealthCheckResilienceService : IHostedService {
  private readonly ILogger<HealthCheckResilienceService>? _logger;
  private readonly HealthCheckService _service;

  public HealthCheckResilienceService(ILogger<HealthCheckResilienceService>? logger, HealthCheckService service) {
    _logger = logger;
    _service = service;
  }

  private const int MAX_RETRIES = 10;
  private const int RETRY_DELAY = 5;

  public async Task WaitForHealthyStatusAsync(CancellationToken cancellationToken = default) {
    var isHealthy = false;

    for (int attempt = 1; attempt <= MAX_RETRIES; attempt++) {
      try {
        var healthReport = await _service.CheckHealthAsync(cancellationToken);

        if (healthReport.Status == HealthStatus.Healthy) {
          _logger?.LogInformation("Health check passed on attempt {Attempt}.", attempt);
          isHealthy = true;
          break;
        }

        _logger?.LogWarning(
          "Health check failed on attempt {Attempt}/{MaxRetries}. Retrying in {RetryDelaySeconds} seconds...",
          attempt,
          MAX_RETRIES,
          RETRY_DELAY
        );

        foreach (var entry in healthReport.Entries) {
          _logger?.LogWarning(
            "Health check entry {Key}: {Status} - {Description}",
            entry.Key,
            entry.Value.Status,
            entry.Value.Description
          );
        }
      }
      catch (Exception ex) {
        _logger?.LogError(
          ex,
          "An exception occurred during health check attempt {Attempt}/{MaxRetries}.",
          attempt,
          MAX_RETRIES
        );
      }

      for (int remainingSeconds = RETRY_DELAY; remainingSeconds > 0; remainingSeconds--) {
        _logger?.LogInformation("Retrying in {RemainingSeconds} second(s)...", remainingSeconds);
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
      }
    }

    if (!isHealthy) {
      _logger?.LogError("-------------------------------------------------------------------------------");
      _logger?.LogError("Health check failed after {MaxRetries:D2} attempts. Application starting as not operational!", MAX_RETRIES);
      _logger?.LogError("-------------------------------------------------------------------------------");
    };
  }

  public async Task RunManuallyAsync(CancellationToken cancellationToken = default) {
    await StartAsync(cancellationToken);
  }

  public async Task StartAsync(CancellationToken cancellationToken) {
    await WaitForHealthyStatusAsync(cancellationToken);
  }

  public Task StopAsync(CancellationToken cancellationToken) {
    throw new NotImplementedException();
  }
}
