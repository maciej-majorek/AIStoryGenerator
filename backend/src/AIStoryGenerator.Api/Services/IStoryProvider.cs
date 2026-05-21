namespace AIStoryGenerator.Api.Services;

/// <summary>
/// Abstraction for AI story generation providers (OpenAI, Anthropic, local models, etc.)
/// </summary>
public interface IStoryProvider
{
    /// <summary>
    /// Generate a story based on the provided request
    /// </summary>
    /// <param name="request">The story request with all preferences</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>Generated story content</returns>
    Task<string> GenerateStoryAsync(Models.StoryRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the name/identifier of the provider
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Check if the provider is available/healthy
    /// </summary>
    Task<bool> IsAvailableAsync();
}
