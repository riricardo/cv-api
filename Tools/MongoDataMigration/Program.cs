using MongoDataMigration;

var apiBaseUrl = ConsolePrompt.ReadValue("API base URL", "http://localhost:5255");
var masterKey = ConsolePrompt.ReadSecret("Master key");
var importData = await ImportDataLoader.LoadAsync();

using var apiClient = new CvApiClient(apiBaseUrl, masterKey);
var runner = new MigrationRunner(apiClient);
await runner.RunAsync(importData);

Console.WriteLine();
Console.WriteLine("Migration finished.");
