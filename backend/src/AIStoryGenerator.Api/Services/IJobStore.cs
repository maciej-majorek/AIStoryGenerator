namespace AIStoryGenerator.Api.Services;

/// <summary>
/// Abstraction for storing and retrieving generation jobs
/// </summary>
public interface IJobStore
{
    /// <summary>
    /// Save or update a job
    /// </summary>
    Task SaveJobAsync(Models.GenerationJob job);

    /// <summary>
    /// Retrieve a job by ID
    /// </summary>
    Task<Models.GenerationJob?> GetJobAsync(Guid jobId);

    /// <summary>
    /// Get all jobs (for a session/user)
    /// </summary>
    Task<IEnumerable<Models.GenerationJob>> GetAllJobsAsync();

    /// <summary>
    /// Delete a job (for cleanup)
    /// </summary>
    Task DeleteJobAsync(Guid jobId);
}
