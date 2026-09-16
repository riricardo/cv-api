using cv_api.Models;

namespace cv_api.Dtos;

public record ResumeByLinkResponse(
    Resume Resume,
    ProfileDetailsResponse? Profile
);
