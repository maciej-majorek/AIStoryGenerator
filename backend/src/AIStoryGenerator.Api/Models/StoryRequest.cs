namespace AIStoryGenerator.Api.Models;

/// <summary>
/// Represents a user's story generation request with all preferences
/// </summary>
public class StoryRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Plot { get; set; } = string.Empty;
    public string Characters { get; set; } = string.Empty;
    public string Setting { get; set; } = string.Empty;
    public StoryFormat Format { get; set; } = StoryFormat.Prose;
    public int Length { get; set; } = 500; // characters
    public string Genre { get; set; } = string.Empty;
    public string Theme { get; set; } = string.Empty;
    public string PointOfView { get; set; } = "Third Person";
    public string DialogueBalance { get; set; } = "Balanced";
    public string AdditionalContext { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Story format enumeration
/// </summary>
public enum StoryFormat
{
    Prose = 0,
    Screenplay = 1,
    Stageplay = 2,
    Poem = 3
}
