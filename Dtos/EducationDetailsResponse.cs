using cv_api.Models;

namespace cv_api.Dtos;

public record EducationDetailsResponse(
    Education Education,
    IReadOnlyCollection<SkillDetailsResponse> Skills
);
