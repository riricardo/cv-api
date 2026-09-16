using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace cv_api.Configuration;

public static class DateTimeOpenApiSchemaTransformer
{
    public static Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        if (context.JsonTypeInfo.Type == typeof(DateTime))
        {
            schema.Type = JsonSchemaType.String;
            schema.Format = "date-time";
        }

        if (context.JsonPropertyInfo?.Name is "createdAt" or "updatedAt")
        {
            schema.ReadOnly = true;
        }

        return Task.CompletedTask;
    }
}
