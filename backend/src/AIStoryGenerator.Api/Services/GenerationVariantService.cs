using Microsoft.Extensions.Logging;

namespace AIStoryGenerator.Api.Services;

/// <summary>
/// Service for handling story generation variants and regeneration history
/// Tracks variants and metadata for regenerated stories
/// </summary>
public interface IGenerationVariantService
{
    /// <summary>
    /// Record a generation variant
    /// </summary>
    Task RecordVariantAsync(Models.GenerationJob job, Models.GeneratedStory story);

    /// <summary>
    /// Get all variants for a request
    /// </summary>
    Task<IEnumerable<Models.GeneratedStory>> GetVariantsAsync(Guid requestId);
}

/// <summary>
/// Implementation of generation variant tracking
/// </summary>
public class GenerationVariantService : IGenerationVariantService
{
    private readonly Dictionary<Guid, List<Models.GeneratedStory>> _variants = new();
    private readonly object _lockObject = new object();
    private readonly ILogger<GenerationVariantService> _logger;

    public GenerationVariantService(ILogger<GenerationVariantService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task RecordVariantAsync(Models.GenerationJob job, Models.GeneratedStory story)
    {
        if (job == null)
            throw new ArgumentNullException(nameof(job));
        if (story == null)
            throw new ArgumentNullException(nameof(story));

        lock (_lockObject)
        {
            if (!_variants.ContainsKey(job.RequestId))
            {
                _variants[job.RequestId] = new List<Models.GeneratedStory>();
            }

            _variants[job.RequestId].Add(story);
            _logger.LogInformation(
                "Recorded variant {VariantCount} for request {RequestId}",
                _variants[job.RequestId].Count,
                job.RequestId);
        }

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Models.GeneratedStory>> GetVariantsAsync(Guid requestId)
    {
        lock (_lockObject)
        {
            if (_variants.TryGetValue(requestId, out var variants))
            {
                return Task.FromResult(variants.AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<Models.GeneratedStory>());
        }
    }
}
