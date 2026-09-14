using cv_api.Repositories;

namespace MongoDataMigration;

public class MigrationRunner
{
    private readonly CvApiClient _apiClient;

    public MigrationRunner(CvApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<MigrationSummary> RunAsync(CancellationToken cancellationToken = default)
    {
        var summary = new MigrationSummary();

        await PostAllAsync("resumes", CvSeedData.Resumes, summary, cancellationToken);
        await PostAllAsync("profiles", CvSeedData.Profiles, summary, cancellationToken);
        await PostAllAsync("personal-info", CvSeedData.PersonalInfo, summary, cancellationToken);
        await PostAllAsync("experiences", CvSeedData.Experiences, summary, cancellationToken);
        await PostAllAsync("education", CvSeedData.Education, summary, cancellationToken);
        await PostAllAsync("projects", CvSeedData.Projects, summary, cancellationToken);
        await PostAllAsync("skills", CvSeedData.Skills, summary, cancellationToken);
        await PostAllAsync("skill-categories", CvSeedData.SkillCategories, summary, cancellationToken);
        await PostAllAsync("spoken-languages", CvSeedData.SpokenLanguages, summary, cancellationToken);

        return summary;
    }

    private async Task PostAllAsync<T>(
        string route,
        IReadOnlyCollection<T> documents,
        MigrationSummary summary,
        CancellationToken cancellationToken
    ) where T : class
    {
        foreach (var document in documents)
        {
            var id = DocumentId.Get(document);
            var result = await _apiClient.CreateAsync(route, document, cancellationToken);

            ApplyResult(route, id, result, summary);
        }
    }

    private static void ApplyResult(
        string route,
        string id,
        MigrationItemResult result,
        MigrationSummary summary
    )
    {
        switch (result.Status)
        {
            case MigrationItemStatus.Inserted:
                summary.Inserted++;
                Console.WriteLine($"Inserted {route}: {id}");
                break;
            case MigrationItemStatus.Ignored:
                summary.Ignored++;
                Console.WriteLine($"Ignored {route}: {id}");
                break;
            case MigrationItemStatus.Failed:
                summary.Failed++;
                Console.WriteLine($"Failed {route}: {id}. Status: {result.StatusCode}. {result.ResponseBody}");
                break;
        }
    }
}
