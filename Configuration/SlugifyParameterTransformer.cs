using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace cv_api.Configuration;

public partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null)
        {
            return null;
        }

        return ControllerNameRegex()
            .Replace(value.ToString()!, "$1-$2")
            .ToLowerInvariant();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex ControllerNameRegex();
}
