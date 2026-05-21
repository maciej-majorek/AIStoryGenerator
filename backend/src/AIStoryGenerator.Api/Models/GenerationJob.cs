namespace AIStoryGenerator.Api.Models;

/// <summary>
/// Represents the lifecycle and status of a story generation job
/// </summary>
public class GenerationJob
{
    public Guid JobId { get; set; } = Guid.NewGuid();
    public Guid RequestId { get; set; }
    public GenerationJobStatus Status { get; set; } = GenerationJobStatus.Queued;
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Generation job status enumeration
/// </summary>
public enum GenerationJobStatus
{
    Queued = 0,
    Running = 1,
    Succeeded = 2,
    Failed = 3
}
