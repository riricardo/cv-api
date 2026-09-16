using cv_api.Models;

namespace cv_api.Dtos;

public record ProjectDetailsResponse(
    Project Project,
    IReadOnlyCollection<SkillDetailsResponse> Skills
);
