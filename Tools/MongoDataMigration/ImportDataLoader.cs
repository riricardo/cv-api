using System.Text.Json;
using cv_api.Models;

namespace MongoDataMigration;

public static class ImportDataLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<ImportData> LoadAsync(CancellationToken cancellationToken = default)
    {
        var importPath = GetImportPath();

        if (!Directory.Exists(importPath))
        {
            throw new DirectoryNotFoundException($"Import path was not found: {importPath}");
        }

        return new ImportData
        {
            SkillCategories = await ReadAsync<SkillCategory>(importPath, cancellationToken),
            Skills = await ReadAsync<Skill>(importPath, cancellationToken),
            SpokenLanguages = await ReadAsync<SpokenLanguage>(importPath, cancellationToken),
            PersonalInfo = await ReadAsync<PersonalInfo>(importPath, cancellationToken),
            Experiences = await ReadAsync<Experience>(importPath, cancellationToken),
            Education = await ReadAsync<Education>(importPath, cancellationToken),
            Projects = await ReadAsync<Project>(importPath, cancellationToken),
            Profiles = await ReadAsync<Profile>(importPath, cancellationToken),
            Resumes = await ReadAsync<Resume>(importPath, cancellationToken)
        };
    }

    private static string GetImportPath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var candidates = new[]
        {
            Path.Combine(currentDirectory, "JsonData"),
            Path.Combine(currentDirectory, "Tools", "MongoDataMigration", "JsonData")
        };

        foreach (var candidate in candidates)
        {
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
        }

        return candidates[0];
    }

    private static async Task<IReadOnlyCollection<T>> ReadAsync<T>(
        string importPath,
        CancellationToken cancellationToken
    )
    {
        var filePath = Path.Combine(importPath, $"{typeof(T).Name}.json");

        if (!File.Exists(filePath))
        {
            return [];
        }

        await using var stream = File.OpenRead(filePath);
        using var jsonDocument = await JsonDocument.ParseAsync(
            stream,
            cancellationToken: cancellationToken
        );

        return jsonDocument.RootElement.ValueKind switch
        {
            JsonValueKind.Array => JsonSerializer.Deserialize<List<T>>(
                jsonDocument.RootElement.GetRawText(),
                JsonOptions
            ) ?? [],
            JsonValueKind.Object => DeserializeSingleDocument<T>(jsonDocument.RootElement),
            _ => throw new InvalidOperationException(
                $"Import file must contain a JSON object or array: {filePath}"
            )
        };
    }

    private static IReadOnlyCollection<T> DeserializeSingleDocument<T>(JsonElement rootElement)
    {
        var document = JsonSerializer.Deserialize<T>(
            rootElement.GetRawText(),
            JsonOptions
        );

        return document is null ? [] : [document];
    }
}
