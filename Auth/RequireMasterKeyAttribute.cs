using Microsoft.AspNetCore.Mvc;

namespace cv_api.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireMasterKeyAttribute : TypeFilterAttribute
{
    public RequireMasterKeyAttribute()
        : base(typeof(MasterKeyAuthorizationFilter))
    {
    }
}
