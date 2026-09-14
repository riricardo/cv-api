using cv_api.Models;

namespace MongoDataMigration;

public class ImportData
{
    public IReadOnlyCollection<Resume> Resumes { get; init; } = [];

    public IReadOnlyCollection<Profile> Profiles { get; init; } = [];

    public IReadOnlyCollection<PersonalInfo> PersonalInfo { get; init; } = [];

    public IReadOnlyCollection<Experience> Experiences { get; init; } = [];

    public IReadOnlyCollection<Education> Education { get; init; } = [];

    public IReadOnlyCollection<Project> Projects { get; init; } = [];

    public IReadOnlyCollection<Skill> Skills { get; init; } = [];

    public IReadOnlyCollection<SkillCategory> SkillCategories { get; init; } = [];

    public IReadOnlyCollection<SpokenLanguage> SpokenLanguages { get; init; } = [];
}
