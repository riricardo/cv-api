using cv_api.Models;

namespace cv_api.Dtos;

public record ExperienceDetailsResponse(
    Experience Experience,
    IReadOnlyCollection<SkillDetailsResponse> Skills
);
