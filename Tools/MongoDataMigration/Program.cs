using MongoDataMigration;

var apiBaseUrl = ConsolePrompt.ReadValue("API base URL", "http://localhost:5255");
var masterKey = ConsolePrompt.ReadSecret("Master key");

using var apiClient = new CvApiClient(apiBaseUrl, masterKey);
var runner = new MigrationRunner(apiClient);
var summary = await runner.RunAsync();

Console.WriteLine();
Console.WriteLine($"Migration completed. Inserted: {summary.Inserted}. Ignored: {summary.Ignored}. Failed: {summary.Failed}.");
