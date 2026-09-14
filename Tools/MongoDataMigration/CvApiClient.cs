using System.Net;
using System.Net.Http.Json;

namespace MongoDataMigration;

public class CvApiClient : IDisposable
{
    private readonly HttpClient _httpClient;

    public CvApiClient(string baseUrl, string masterKey)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
        _httpClient.DefaultRequestHeaders.Add("master_key", masterKey);
    }

    public async Task<MigrationItemResult> CreateAsync<T>(
        string route,
        T document,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/{route}", document, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        return response.StatusCode switch
        {
            HttpStatusCode.Created => MigrationItemResult.Inserted(),
            HttpStatusCode.Conflict => MigrationItemResult.Ignored(),
            _ => MigrationItemResult.Failed((int)response.StatusCode, responseBody)
        };
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
