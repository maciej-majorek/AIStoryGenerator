using Microsoft.Extensions.Logging;

namespace AIStoryGenerator.Api.Services;

/// <summary>
/// Telemetry service for tracking generation events and metrics
/// </summary>
public interface ITelemetryService
{
    /// <summary>
    /// Record a generation event
    /// </summary>
    void RecordGenerationEvent(GenerationEventData eventData);

    /// <summary>
    /// Get telemetry metrics
    /// </summary>
    TelemetryMetrics GetMetrics();
}

/// <summary>
/// Generation event data
/// </summary>
public class GenerationEventData
{
    public Guid RequestId { get; set; }
    public Guid JobId { get; set; }
    public string EventType { get; set; } = string.Empty; // "Started", "Completed", "Failed", "Regenerated"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public long? DurationMs { get; set; }
    public string? Metadata { get; set; }
}

/// <summary>
/// Telemetry metrics aggregate
/// </summary>
public class TelemetryMetrics
{
    public int TotalGenerations { get; set; }
    public int SuccessfulGenerations { get; set; }
    public int FailedGenerations { get; set; }
    public int RegenerationCount { get; set; }
    public double AverageDurationMs { get; set; }
}

/// <summary>
/// Implementation of telemetry service
/// </summary>
public class TelemetryService : ITelemetryService
{
    private readonly List<GenerationEventData> _events = new();
    private readonly object _lockObject = new object();
    private readonly ILogger<TelemetryService> _logger;

    public TelemetryService(ILogger<TelemetryService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void RecordGenerationEvent(GenerationEventData eventData)
    {
        if (eventData == null)
            throw new ArgumentNullException(nameof(eventData));

        lock (_lockObject)
        {
            _events.Add(eventData);
            _logger.LogInformation(
                "Telemetry: {EventType} for request {RequestId} at {Timestamp}",
                eventData.EventType,
                eventData.RequestId,
                eventData.Timestamp);
        }
    }

    public TelemetryMetrics GetMetrics()
    {
        lock (_lockObject)
        {
            var totalEvents = _events.Count;
            var completed = _events.Count(e => e.EventType == "Completed");
            var failed = _events.Count(e => e.EventType == "Failed");
            var regenerated = _events.Count(e => e.EventType == "Regenerated");
            var avgDuration = _events
                .Where(e => e.DurationMs.HasValue)
                .Average(e => (double)e.DurationMs.GetValueOrDefault(0));

            return new TelemetryMetrics
            {
                TotalGenerations = totalEvents,
                SuccessfulGenerations = completed,
                FailedGenerations = failed,
                RegenerationCount = regenerated,
                AverageDurationMs = double.IsNaN(avgDuration) ? 0 : avgDuration
            };
        }
    }
}
