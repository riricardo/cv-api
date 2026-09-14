using cv_api.Models;

namespace cv_api.Repositories;

public static class MongoCollectionNames
{
    public static string GetName<T>() => typeof(T) switch
    {
        var type when type == typeof(Resume) => "resumes",
        var type when type == typeof(Profile) => "profiles",
        var type when type == typeof(PersonalInfo) => "personalInfo",
        var type when type == typeof(Experience) => "experiences",
        var type when type == typeof(Education) => "education",
        var type when type == typeof(Project) => "projects",
        var type when type == typeof(Skill) => "skills",
        var type when type == typeof(SkillCategory) => "skillCategories",
        var type when type == typeof(SpokenLanguage) => "spokenLanguages",
        _ => throw new InvalidOperationException($"No MongoDB collection configured for {typeof(T).Name}.")
    };
}
