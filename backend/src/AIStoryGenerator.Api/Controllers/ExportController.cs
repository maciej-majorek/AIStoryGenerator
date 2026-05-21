using Microsoft.AspNetCore.Mvc;
using AIStoryGenerator.Api.Services;

namespace AIStoryGenerator.Api.Controllers;

/// <summary>
/// API controller for story export endpoints
/// </summary>
[ApiController]
[Route("api/v1")]
public class ExportController : ControllerBase
{
    private readonly IJobStore _jobStore;
    private readonly ILogger<ExportController> _logger;

    public ExportController(IJobStore jobStore, ILogger<ExportController> logger)
    {
        _jobStore = jobStore ?? throw new ArgumentNullException(nameof(jobStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Download a generated story as a text file
    /// </summary>
    /// <param name="jobId">The job ID of the generated story</param>
    /// <returns>The story content as a downloadable file</returns>
    [HttpGet("export/{jobId}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DownloadStory(Guid jobId)
    {
        try
        {
            var job = await _jobStore.GetJobAsync(jobId);
            if (job == null)
            {
                _logger.LogWarning("Export requested for non-existent job {JobId}", jobId);
                return NotFound(new { error = "Job not found" });
            }

            // In a real implementation, we would fetch the story content from storage
            // For now, return a placeholder response
            var filename = $"story-{jobId:N}.txt";
            var placeholder = "Story content would be downloaded here.";

            return File(
                System.Text.Encoding.UTF8.GetBytes(placeholder),
                "text/plain",
                filename);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading story {JobId}", jobId);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Failed to download story" });
        }
    }

    /// <summary>
    /// Get story metadata for export
    /// </summary>
    [HttpGet("export/{jobId}/metadata")]
    [ProducesResponseType(typeof(StoryMetadataResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStoryMetadata(Guid jobId)
    {
        var job = await _jobStore.GetJobAsync(jobId);
        if (job == null)
        {
            return NotFound(new { error = "Job not found" });
        }

        var metadata = new StoryMetadataResponse
        {
            JobId = job.JobId,
            Status = job.Status.ToString(),
            CreatedAt = job.StartedAt,
            CompletedAt = job.CompletedAt
        };

        return Ok(metadata);
    }

    /// <summary>
    /// Response model for story metadata
    /// </summary>
    public class StoryMetadataResponse
    {
        public Guid JobId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
