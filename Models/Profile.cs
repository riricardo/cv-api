namespace cv_api.Models;

public class Profile
{
    public string? Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Language { get; set; }
    public required string Name { get; set; }
    public required string PersonalInfoId { get; set; }
    public required string ProfessionalSummary { get; set; }
    public List<ProfileExperience> Experiences { get; set; } = [];
    public List<string> EducationIds { get; set; } = [];
    public List<string> ProjectIds { get; set; } = [];
    public List<string> SkillIds { get; set; } = [];
    public List<string> SpokenLanguageIds { get; set; } = [];
}
