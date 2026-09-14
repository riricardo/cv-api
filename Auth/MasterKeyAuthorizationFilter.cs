using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BCryptNet = BCrypt.Net.BCrypt;

namespace cv_api.Auth;

public class MasterKeyAuthorizationFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public MasterKeyAuthorizationFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var masterKey = context.HttpContext.Request.Headers["X-MASTER-KEY"].ToString();
        var masterKeyHash = _configuration["MASTER_KEY"];

        if (string.IsNullOrWhiteSpace(masterKeyHash))
        {
            throw new InvalidOperationException("MASTER_KEY is not configured.");
        }

        if (string.IsNullOrWhiteSpace(masterKey) || !BCryptNet.Verify(masterKey, masterKeyHash))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
