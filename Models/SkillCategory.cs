namespace cv_api.Models;

public class SkillCategory
{
    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Language { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
}
