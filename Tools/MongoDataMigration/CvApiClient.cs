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

    public async Task CreateAsync<T>(
        string route,
        T document,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/{route}", document, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CreateResult>(cancellationToken);

            if (!string.IsNullOrWhiteSpace(result?.Id))
            {
                DocumentId.Set(document, result.Id);
            }

            return;
        }

        var id = DocumentId.GetOrUnknown(document);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        Console.WriteLine($"Failed {route}: {id}. Status: {(int)response.StatusCode}. {responseBody}");
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private record CreateResult([property: JsonPropertyName("id")] string Id);
}
