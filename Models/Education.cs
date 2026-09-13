namespace cv_api.Models;

public class Education
{
    public required string Id { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? TranslationGroupId { get; set; }
    public string? Language { get; set; }
    public required string Institution { get; set; }
    public required string Degree { get; set; }
    public required string Location { get; set; }
    public required string StartDate { get; set; }
    public required string EndDate { get; set; }
    public required string Description { get; set; }
    public List<Highlight> Highlights { get; set; } = [];
    public List<string> SkillIds { get; set; } = [];
}
