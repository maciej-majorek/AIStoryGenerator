namespace AIStoryGenerator.Api.Services;

/// <summary>
/// Local mock story provider for development and testing
/// Generates placeholder stories without requiring external API calls
/// </summary>
public class LocalMockStoryProvider : IStoryProvider
{
    public string ProviderName => "LocalMockProvider";

    public Task<bool> IsAvailableAsync()
    {
        return Task.FromResult(true);
    }

    public Task<string> GenerateStoryAsync(Models.StoryRequest request, CancellationToken cancellationToken = default)
    {
        // Generate a mock story based on the request parameters
        var story = GenerateMockStory(request);
        return Task.FromResult(story);
    }

    private string GenerateMockStory(Models.StoryRequest request)
    {
        var formatDescriptor = request.Format switch
        {
            Models.StoryFormat.Screenplay => "[SCENE: Interior, {setting}]\n[Characters: {characters}]\n{content}",
            Models.StoryFormat.Stageplay => "ACT I\nScene 1: {setting}\n[{characters} enter]\n{content}",
            Models.StoryFormat.Poem => "# {title}\n\n{content}",
            _ => "{content}" // Prose (default)
        };

        var mockContent = $"""
            # A Story About {request.Plot}

            In the setting of {request.Setting}, {request.Characters} find themselves facing a challenge.

            Genre: {request.Genre}
            Theme: {request.Theme}
            Point of View: {request.PointOfView}
            
            This is a mock-generated story for development purposes. 
            The story respects the requested format ({request.Format}), 
            approximate length ({request.Length} characters), 
            and includes dialogue balance of {request.DialogueBalance}.
            
            {request.AdditionalContext}
            
            (This placeholder story demonstrates the story generation pipeline structure)
            """;

        return mockContent.Length > request.Length 
            ? mockContent.Substring(0, request.Length) 
            : mockContent;
    }
}
