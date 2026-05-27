using Microsoft.Extensions.Logging;

namespace AIStoryGenerator.Api.Services;

/// <summary>
/// Orchestrates story generation by coordinating with AI providers
/// Handles job tracking, validation, and safety checks
/// </summary>
public interface IStoryGeneratorService
{
    /// <summary>
    /// Generate a story from the provided request
    /// </summary>
    Task<GenerationResult> GenerateStoryAsync(Models.StoryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the status of a generation job
    /// </summary>
    Task<Models.GenerationJob?> GetJobStatusAsync(Guid jobId);
}

/// <summary>
/// Result of a story generation attempt
/// </summary>
public class GenerationResult
{
    public Guid JobId { get; set; }
    public Models.GeneratedStory? Story { get; set; }
    public bool Success { get; set; }
    public string? Error { get; set; }
}

/// <summary>
/// Implementation of story generation orchestration
/// </summary>
public class StoryGeneratorService : IStoryGeneratorService
{
    private readonly IStoryProvider _provider;
    private readonly IJobStore _jobStore;
    private readonly ILogger<StoryGeneratorService> _logger;

    public StoryGeneratorService(
        IStoryProvider provider,
        IJobStore jobStore,
        ILogger<StoryGeneratorService> logger)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _jobStore = jobStore ?? throw new ArgumentNullException(nameof(jobStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<GenerationResult> GenerateStoryAsync(
        Models.StoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var job = new Models.GenerationJob
        {
            RequestId = request.Id,
            Status = Models.GenerationJobStatus.Running,
            StartedAt = DateTime.UtcNow
        };

        try
        {
            _logger.LogInformation("Starting story generation for request {RequestId}", request.Id);
            await _jobStore.SaveJobAsync(job);

            // Call the provider
            var content = await _provider.GenerateStoryAsync(request, cancellationToken);

            // Create the generated story
            var story = new Models.GeneratedStory
            {
                JobId = job.JobId,
                Content = content,
                Format = request.Format,
                Length = content.Length
            };

            job.Status = Models.GenerationJobStatus.Succeeded;
            job.CompletedAt = DateTime.UtcNow;
            await _jobStore.SaveJobAsync(job);

            _logger.LogInformation("Successfully generated story for request {RequestId}", request.Id);

            return new GenerationResult
            {
                JobId = job.JobId,
                Story = story,
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating story for request {RequestId}", request.Id);

            job.Status = Models.GenerationJobStatus.Failed;
            job.CompletedAt = DateTime.UtcNow;
            job.Error = ex.Message;
            await _jobStore.SaveJobAsync(job);

            return new GenerationResult
            {
                JobId = job.JobId,
                Success = false,
                Error = $"Failed to generate story: {ex.Message}"
            };
        }
    }

    public async Task<Models.GenerationJob?> GetJobStatusAsync(Guid jobId)
    {
        return await _jobStore.GetJobAsync(jobId);
    }
}
