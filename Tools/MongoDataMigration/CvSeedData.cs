using cv_api.Models;

namespace MongoDataMigration;

public static class CvSeedData
{
    public static IReadOnlyCollection<Resume> Resumes { get; } = [];

    public static IReadOnlyCollection<Profile> Profiles { get; } = [];

    public static IReadOnlyCollection<PersonalInfo> PersonalInfo { get; } = [];

    public static IReadOnlyCollection<Experience> Experiences { get; } = [];

    public static IReadOnlyCollection<Education> Education { get; } = [];

    public static IReadOnlyCollection<Project> Projects { get; } = [];

    public static IReadOnlyCollection<Skill> Skills { get; } = [];

    public static IReadOnlyCollection<SkillCategory> SkillCategories { get; } = [];

    public static IReadOnlyCollection<SpokenLanguage> SpokenLanguages { get; } = [];
}
