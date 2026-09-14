namespace cv_api.Models;

public class Skill
{
    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public required string CategoryId { get; set; }
}
