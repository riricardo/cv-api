namespace cv_api.Models;

public class Resume
{
    public required string Id { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public required string LinkId { get; set; }
    public string? Language { get; set; }
    public required string ProfileId { get; set; }
    public List<string> WhyText { get; set; } = [];
    public ResumeDetails? Details { get; set; }
}
