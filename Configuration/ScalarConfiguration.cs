using Scalar.AspNetCore;

namespace cv_api.Configuration;

public static class ScalarConfiguration
{
    public static void Configure(ScalarOptions options)
    {
        options.HideModels = true;
        options.DocumentDownloadType = DocumentDownloadType.None;
        options.HideClientButton = true;
        options.HideDarkModeToggle = true;
        options.HideSearch = true;
        options.ShowSidebar = false;
        options.Title = "CV API";
        options.Layout = ScalarLayout.Classic;
        options
            .EnablePersistentAuthentication()
            .AddPreferredSecuritySchemes("MasterKey");
    }
}
