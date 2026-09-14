using MongoDB.Bson;

namespace cv_api.Data;

public static class MongoHealth
{
    public static async Task<IResult> Ping(MongoDbContext mongoDbContext)
    {
        try
        {
            await mongoDbContext.Database.RunCommandAsync<BsonDocument>(
                new BsonDocument("ping", 1)
            );

            return Results.Ok("database connected");
        }
        catch
        {
            return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
        }
    }
}
