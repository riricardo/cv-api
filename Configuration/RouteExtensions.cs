using Scalar.AspNetCore;
using cv_api.Data;

namespace cv_api.Configuration;

public static class RouteExtensions
{
    public static void MapAppRoutes(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference("/docs", ScalarConfiguration.Configure).ExcludeFromDescription();
        app.MapGet("/health", MongoHealth.Ping);
        app.MapControllers();
        app.MapGet("/", () => Results.Redirect("/docs")).ExcludeFromDescription();
    }
}
