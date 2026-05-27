namespace AIStoryGenerator.Api.Services;

using System.Net.Http.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Story provider that uses DIAL AI engine for real story generation
/// Connects to Azure OpenAI-compatible endpoint for generating stories
/// </summary>
public class LocalMockStoryProvider : IStoryProvider
{
    private readonly ILogger<LocalMockStoryProvider> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    private const string DialEndpoint = "https://ai-proxy.lab.epam.com";
    private const string DialDeployment = "gpt-4";
    private const string ApiVersion = "2024-02-01";

    public string ProviderName => "DialAIProvider";

    public LocalMockStoryProvider(ILogger<LocalMockStoryProvider> logger)
    {
        _logger = logger;

        // Get API key from environment variable
        _apiKey = Environment.GetEnvironmentVariable("DIAL_API_KEY") ?? string.Empty;
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("DIAL_API_KEY environment variable is not set. Please configure the API key.");
        }

        // Initialize HTTP client
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
    }

    public Task<bool> IsAvailableAsync()
    {
        try
        {
            return Task.FromResult(!string.IsNullOrEmpty(_apiKey));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking DIAL AI availability");
            return Task.FromResult(false);
        }
    }

    public async Task<string> GenerateStoryAsync(Models.StoryRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var prompt = BuildStoryPrompt(request);

            _logger.LogInformation("Generating story with DIAL AI for plot: {Plot}", request.Plot);

            var url = $"{DialEndpoint}/openai/deployments/{DialDeployment}/chat/completions?api-version={ApiVersion}";

            var requestBody = new ChatCompletionRequest
            {
                Messages = new[]
                {
                    new ChatMessage { Role = "system", Content = "You are a creative storyteller. Generate engaging and well-written stories based on the provided parameters." },
                    new ChatMessage { Role = "user", Content = prompt }
                },
                MaxTokens = CalculateMaxTokens(request.Length),
                Temperature = 0.7f,
                TopP = 0.9f
            };

            var response = await _httpClient.PostAsJsonAsync(url, requestBody, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ChatCompletionResponse>(cancellationToken: cancellationToken);
            var story = result?.Choices?[0]?.Message?.Content;

            _logger.LogInformation("Story generated successfully. Length: {Length}", story?.Length ?? 0);

            return story ?? throw new InvalidOperationException("No content received from DIAL AI");
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Story generation was cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating story with DIAL AI");
            throw new InvalidOperationException("Failed to generate story using DIAL AI", ex);
        }
    }

    private string BuildStoryPrompt(Models.StoryRequest request)
    {
        var formatInstructions = request.Format switch
        {
            Models.StoryFormat.Screenplay => "Format the story as a screenplay with scene headings, action lines, character names, and dialogue.",
            Models.StoryFormat.Stageplay => "Format the story as a stage play with acts, scenes, stage directions, and dialogue.",
            Models.StoryFormat.Poem => "Format the story as a poem with appropriate line breaks and stanzas.",
            _ => "Write the story as prose narrative."
        };

        var povInstructions = request.PointOfView switch
        {
            "First Person" => "Write from the first-person perspective.",
            "Second Person" => "Write from the second-person perspective.",
            "Third Person Limited" => "Write from the third-person limited perspective.",
            "Third Person Omniscient" => "Write from the third-person omniscient perspective.",
            _ => ""
        };

        var prompt = $"""
            Create a {request.Genre} story with the following parameters:

            Plot: {request.Plot}
            Setting: {request.Setting}
            Characters: {request.Characters}
            Theme: {request.Theme}
            Approximate Length: {request.Length} characters
            Dialogue Balance: {request.DialogueBalance}% dialogue

            Format Instructions:
            {formatInstructions}

            Point of View:
            {povInstructions}

            Additional Context:
            {request.AdditionalContext}

            Please generate a compelling and creative story based on these parameters.
            """;

        return prompt;
    }

    private int CalculateMaxTokens(int characterLength)
    {
        // Rough estimate: 1 token ≈ 4 characters, add buffer for overhead
        var estimatedTokens = (characterLength / 4) + 100;
        // Cap at reasonable maximum
        return Math.Min(estimatedTokens, 4000);
    }

    private class ChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }

    private class ChatCompletionRequest
    {
        [JsonPropertyName("messages")]
        public ChatMessage[] Messages { get; set; } = Array.Empty<ChatMessage>();

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonPropertyName("temperature")]
        public float Temperature { get; set; }

        [JsonPropertyName("top_p")]
        public float TopP { get; set; }
    }

    private class ChatCompletionResponse
    {
        [JsonPropertyName("choices")]
        public Choice[]? Choices { get; set; }
    }

    private class Choice
    {
        [JsonPropertyName("message")]
        public ChatMessage? Message { get; set; }
    }
}
