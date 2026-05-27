namespace AIStoryGenerator.Api.Validators;

/// <summary>
/// Validation logic for story generation requests
/// </summary>
public static class StoryRequestValidator
{
    private const int MinLength = 50;
    private const int MaxLength = 5000;

    /// <summary>
    /// Validate a story request
    /// </summary>
    /// <param name="request">The request to validate</param>
    /// <returns>Validation result with any errors</returns>
    public static ValidationResult Validate(Models.StoryRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Plot))
            errors.Add("Plot is required");

        if (string.IsNullOrWhiteSpace(request.Setting))
            errors.Add("Setting is required");

        if (string.IsNullOrWhiteSpace(request.Characters))
            errors.Add("Characters are required");

        if (request.Length < MinLength)
            errors.Add($"Length must be at least {MinLength} characters");

        if (request.Length > MaxLength)
            errors.Add($"Length cannot exceed {MaxLength} characters");

        if (!Enum.IsDefined(typeof(Models.StoryFormat), request.Format))
            errors.Add("Invalid story format");

        return new ValidationResult
        {
            IsValid = errors.Count == 0,
            Errors = errors
        };
    }

    /// <summary>
    /// Validation result
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
