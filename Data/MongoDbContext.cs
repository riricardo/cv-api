using MongoDB.Driver;

namespace cv_api.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration["MONGODB_URI"];

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "MONGODB_URI is not configured."
            );
        }

        var mongoUrl = new MongoUrl(connectionString);

        if (string.IsNullOrWhiteSpace(mongoUrl.DatabaseName))
        {
            throw new InvalidOperationException(
                "MONGODB_URI must contain a database name."
            );
        }

        var client = new MongoClient(mongoUrl);

        _database = client.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoDatabase Database => _database;
}