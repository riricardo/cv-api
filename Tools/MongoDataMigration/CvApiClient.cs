using System.Net.Http.Json;
using System.Text.Json.Serialization;
using cv_api.Repositories;

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
        _httpClient.DefaultRequestHeaders.Add("X-MASTER-KEY", masterKey);
    }

    public async Task<MigrationLogResult> CreateAsync<T>(
        string route,
        T document,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        var id = DocumentId.GetOrUnknown(document);
        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsJsonAsync($"/api/{route}", document, cancellationToken);
        }
        catch (Exception exception)
        {
            return new MigrationLogResult(route, id, "ERROR", true, exception.Message);
        }

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CreateResult>(cancellationToken);

            if (!string.IsNullOrWhiteSpace(result?.Id))
            {
                DocumentId.Set(document, result.Id);
                id = result.Id;
            }

            return new MigrationLogResult(route, id, result?.Status ?? "inserted", false);
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            var result = await response.Content.ReadFromJsonAsync<CreateResult>(cancellationToken);

            return new MigrationLogResult(route, result?.Id ?? id, result?.Status ?? "ignored", false);
        }

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        return new MigrationLogResult(
            route,
            id,
            "ERROR",
            true,
            $"HTTP {(int)response.StatusCode} {response.StatusCode}: {responseBody}"
        );
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private record CreateResult(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("status")] string Status
    );
}
