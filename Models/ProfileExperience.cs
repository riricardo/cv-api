namespace cv_api.Models;

public class ProfileExperience
{
    public required string ExperienceId { get; set; }
    public bool Print { get; set; }
    public List<string> HighlightIds { get; set; } = [];
    public List<string>? DownloadHighlightIds { get; set; }
}
