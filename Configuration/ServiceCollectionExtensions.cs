using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using cv_api.Data;
using cv_api.Repositories;
using cv_api.Auth;

namespace cv_api.Configuration;

public static class ServiceCollectionExtensions
{
    private const string MasterKeySecurityScheme = "MasterKey";
    private const string MasterKeyHeaderName = "master_key";

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                document.Components.SecuritySchemes[MasterKeySecurityScheme] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = MasterKeyHeaderName,
                    In = ParameterLocation.Header,
                    Description = "Master key required for write operations."
                };

                return Task.CompletedTask;
            });

            options.AddOperationTransformer((operation, context, cancellationToken) =>
            {
                if (!RequiresMasterKey(context))
                {
                    return Task.CompletedTask;
                }

                operation.Security ??= [];
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecuritySchemeReference(
                            MasterKeySecurityScheme,
                            context.Document,
                            externalResource: null
                        )
                    ] = []
                });

                return Task.CompletedTask;
            });
        });

        services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        });
        services.AddSingleton<MongoDbContext>();
        services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

        return services;
    }

    private static bool RequiresMasterKey(OpenApiOperationTransformerContext context)
    {
        return context.Description.ActionDescriptor.EndpointMetadata
                .OfType<RequireMasterKeyAttribute>()
                .Any()
            || context.Description.ActionDescriptor.FilterDescriptors
                .Any(filter => filter.Filter is RequireMasterKeyAttribute);
    }
}
