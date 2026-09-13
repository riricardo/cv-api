namespace cv_api.Models;

public class PersonalInfo
{
    public required string Id { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? TranslationGroupId { get; set; }
    public string? Language { get; set; }
    public required string Name { get; set; }
    public required string FullName { get; set; }
    public required string Location { get; set; }
    public required string DisplayLocation { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Nationality { get; set; }
    public required string ProfessionalDescription { get; set; }
    public required string PageTitle { get; set; }
    public required string WhyTitle { get; set; }
    public required string GitHubUrl { get; set; }
    public required string LinkedInUrl { get; set; }
    public required string PortfolioUrl { get; set; }
}
