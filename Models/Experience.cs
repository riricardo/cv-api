namespace cv_api.Models;

public class Experience
{
    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Language { get; set; }
    public required string Company { get; set; }
    public required string Role { get; set; }
    public required string Location { get; set; }
    public required string Description { get; set; }
    public required string StartDate { get; set; }
    public required string EndDate { get; set; }
    public List<Highlight> Highlights { get; set; } = [];
    public List<string> SkillIds { get; set; } = [];
}
