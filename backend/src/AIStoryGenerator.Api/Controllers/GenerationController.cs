using Microsoft.AspNetCore.Mvc;
using AIStoryGenerator.Api.Models;
using AIStoryGenerator.Api.Services;
using AIStoryGenerator.Api.Validators;

namespace AIStoryGenerator.Api.Controllers;

/// <summary>
/// API controller for story generation endpoints
/// </summary>
[ApiController]
[Route("api/v1")]
public class GenerationController : ControllerBase
{
    private readonly IStoryGeneratorService _generationService;
    private readonly ILogger<GenerationController> _logger;

    public GenerationController(
        IStoryGeneratorService generationService,
        ILogger<GenerationController> logger)
    {
        _generationService = generationService ?? throw new ArgumentNullException(nameof(generationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Generate a story based on user preferences
    /// </summary>
    /// <param name="request">Story generation request with all preferences</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Generated story and job ID</returns>
    [HttpPost("generate")]
    [ProducesResponseType(typeof(GenerateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetail), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetail), StatusCodes.Status413PayloadTooLarge)]
    [ProducesResponseType(typeof(ErrorDetail), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Generate(
        [FromBody] StoryRequest request,
        CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = StoryRequestValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Invalid story request: {Errors}", string.Join(", ", validationResult.Errors));
            return BadRequest(new ErrorDetail
            {
                Message = "Invalid request",
                Errors = validationResult.Errors
            });
        }

        try
        {
            // Generate the story
            var result = await _generationService.GenerateStoryAsync(request, cancellationToken);

            if (!result.Success)
            {
                _logger.LogError("Story generation failed: {Error}", result.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDetail
                {
                    Message = result.Error ?? "Unknown error during generation"
                });
            }

            return Ok(new GenerateResponse
            {
                JobId = result.JobId,
                Story = result.Story
            });
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Story generation was cancelled");
            return StatusCode(StatusCodes.Status408RequestTimeout, new ErrorDetail
            {
                Message = "Request timeout: generation took too long"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during story generation");
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDetail
            {
                Message = "An unexpected error occurred"
            });
        }
    }

    /// <summary>
    /// Get the status of a generation job
    /// </summary>
    /// <param name="jobId">The job ID to check</param>
    /// <returns>Job status information</returns>
    [HttpGet("generate/{jobId}")]
    [ProducesResponseType(typeof(GenerationJob), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetail), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStatus(Guid jobId)
    {
        var job = await _generationService.GetJobStatusAsync(jobId);
        if (job == null)
        {
            return NotFound(new ErrorDetail
            {
                Message = $"Job {jobId} not found"
            });
        }

        return Ok(job);
    }

    /// <summary>
    /// Response model for generation endpoint
    /// </summary>
    public class GenerateResponse
    {
        public Guid JobId { get; set; }
        public GeneratedStory? Story { get; set; }
    }

    /// <summary>
    /// Error detail response model
    /// </summary>
    public class ErrorDetail
    {
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
    }
}
