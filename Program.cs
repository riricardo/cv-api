using Scalar.AspNetCore;
using cv_api.Data;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<MongoDbContext>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference("/docs", options =>
{
    options.OpenApiRoutePattern = "/openapi/v1.json";
}).ExcludeFromDescription();

app.MapGet("/api/health/mongodb", static async (MongoDbContext mongoDbContext) =>
{
    await mongoDbContext.Database.RunCommandAsync<BsonDocument>(
        new BsonDocument("ping", 1)
    );

    return Results.Ok(new
    {
        status = "ok",
        database = "connected"
    });
});

app.MapGet("/", () => Results.Redirect("/docs")).ExcludeFromDescription();

app.UseHttpsRedirection();

app.Run();
