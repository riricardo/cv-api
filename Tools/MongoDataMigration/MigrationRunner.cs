namespace MongoDataMigration;

public class MigrationRunner
{
    private readonly CvApiClient _apiClient;

    public MigrationRunner(CvApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task RunAsync(ImportData importData, CancellationToken cancellationToken = default)
    {
        await PostAllAsync("skill-categories", importData.SkillCategories, cancellationToken);
        await PostAllAsync("skills", importData.Skills, cancellationToken);
        await PostAllAsync("spoken-languages", importData.SpokenLanguages, cancellationToken);
        await PostAllAsync("personal-info", importData.PersonalInfo, cancellationToken);
        await PostAllAsync("experiences", importData.Experiences, cancellationToken);
        await PostAllAsync("education", importData.Education, cancellationToken);
        await PostAllAsync("projects", importData.Projects, cancellationToken);
        await PostAllAsync("profiles", importData.Profiles, cancellationToken);
        await PostAllAsync("resumes", importData.Resumes, cancellationToken);
    }

    private async Task PostAllAsync<T>(
        string route,
        IReadOnlyCollection<T> documents,
        CancellationToken cancellationToken
    ) where T : class
    {
        foreach (var document in documents)
        {
            await _apiClient.CreateAsync(route, document, cancellationToken);
        }
    }
}
