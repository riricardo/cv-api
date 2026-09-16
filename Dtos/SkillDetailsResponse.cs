using cv_api.Models;

namespace cv_api.Dtos;

public record SkillDetailsResponse(
    Skill Skill,
    SkillCategory? Category
);
