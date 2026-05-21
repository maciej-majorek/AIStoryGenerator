namespace AIStoryGenerator.Api.Models;

/// <summary>
/// Represents a generated story output
/// </summary>
public class GeneratedStory
{
    public Guid StoryId { get; set; } = Guid.NewGuid();
    public Guid JobId { get; set; }
    public string Content { get; set; } = string.Empty;
    public StoryFormat Format { get; set; } = StoryFormat.Prose;
    public int Length { get; set; } // character count
    public int TokensUsed { get; set; } = 0; // if provider reports token usage
    public string[] SafetyFlags { get; set; } = Array.Empty<string>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
