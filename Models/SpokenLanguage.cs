namespace cv_api.Models;

public class SpokenLanguage
{
    public required string Id { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? TranslationGroupId { get; set; }
    public string? Language { get; set; }
    public required string Name { get; set; }
    public required string Proficiency { get; set; }
}
