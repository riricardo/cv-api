using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference("/scalar/v1", options =>
{
    options.OpenApiRoutePattern = "/openapi/v1.json";
});

app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.UseHttpsRedirection();

app.Run();
