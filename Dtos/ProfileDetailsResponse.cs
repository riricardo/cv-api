using cv_api.Models;

namespace cv_api.Dtos;

public record ProfileDetailsResponse(
    Profile Profile,
    PersonalInfo? PersonalInfo,
    IReadOnlyCollection<ExperienceDetailsResponse> Experiences,
    IReadOnlyCollection<EducationDetailsResponse> Education,
    IReadOnlyCollection<ProjectDetailsResponse> Projects,
    IReadOnlyCollection<SkillDetailsResponse> Skills,
    IReadOnlyCollection<SpokenLanguage> SpokenLanguages
);
