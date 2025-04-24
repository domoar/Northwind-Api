using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;


namespace Api.Dev;
[ExcludeFromCodeCoverage(Justification = "Development")]
public class NorthiwndMetrics {
  public const string MeterName = "Northwind-Api";

  private readonly Counter<long> _counter;
  private readonly Histogram<double> _duration;

  public NorthiwndMetrics(IMeterFactory meterFactory) {
    var meter = meterFactory.Create(MeterName);
    _counter = meter.CreateCounter<long>("northwind.api.request.count");
    _duration = meter.CreateHistogram<double>("northwind.api.request.duration", "ms");
  }

  public void IncreaseRequestCount() {
    _counter.Add(1);
  }

  public TrackedRequestDuration MeasureRequestDuration() {
    return new TrackedRequestDuration(_duration);
  }
}

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
public class TrackedRequestDuration(Histogram<double> histogram) : IDisposable {
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
  private readonly long _startTime = TimeProvider.System.GetTimestamp();
  private readonly Histogram<double> _histogram = histogram;

  public void Dispose() {
    var elapsedTime = TimeProvider.System.GetElapsedTime(_startTime);
    _histogram.Record(elapsedTime.TotalMilliseconds);
    GC.SuppressFinalize(this);
  }
}
