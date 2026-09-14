using cv_api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices();

var app = builder.Build();

app.MapAppRoutes();

app.UseHttpsRedirection();

app.Run();
