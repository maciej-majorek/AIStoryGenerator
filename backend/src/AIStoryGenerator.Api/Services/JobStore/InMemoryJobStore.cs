using Microsoft.Extensions.Logging;

namespace AIStoryGenerator.Api.Services.JobStore;

/// <summary>
/// In-memory job store for session-only persistence
/// Jobs are lost when the application restarts
/// </summary>
public class InMemoryJobStore : IJobStore
{
    private readonly Dictionary<Guid, Models.GenerationJob> _jobs = new();
    private readonly object _lockObject = new object();
    private readonly ILogger<InMemoryJobStore> _logger;

    public InMemoryJobStore(ILogger<InMemoryJobStore> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task SaveJobAsync(Models.GenerationJob job)
    {
        if (job == null)
            throw new ArgumentNullException(nameof(job));

        lock (_lockObject)
        {
            _jobs[job.JobId] = job;
            _logger.LogDebug("Saved job {JobId} with status {Status}", job.JobId, job.Status);
        }

        return Task.CompletedTask;
    }

    public Task<Models.GenerationJob?> GetJobAsync(Guid jobId)
    {
        lock (_lockObject)
        {
            _jobs.TryGetValue(jobId, out var job);
            return Task.FromResult(job);
        }
    }

    public Task<IEnumerable<Models.GenerationJob>> GetAllJobsAsync()
    {
        lock (_lockObject)
        {
            var jobs = _jobs.Values.ToList().AsEnumerable();
            return Task.FromResult(jobs);
        }
    }

    public Task DeleteJobAsync(Guid jobId)
    {
        lock (_lockObject)
        {
            if (_jobs.Remove(jobId))
            {
                _logger.LogDebug("Deleted job {JobId}", jobId);
            }
        }

        return Task.CompletedTask;
    }
}
