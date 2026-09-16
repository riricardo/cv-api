using Microsoft.AspNetCore.Mvc;
using cv_api.Models;
using cv_api.Queries;
using cv_api.Repositories;

namespace cv_api.Controllers;

public class EducationController : CrudControllerBase<Education>
{
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetDetails(
        string id,
        [FromServices] QueryService queryService,
        CancellationToken cancellationToken
    )
    {
        if (!DocumentId.IsValid(id))
        {
            return BadRequest(new { message = "Id must be a valid GUID." });
        }

        var details = await queryService.GetEducationDetailsAsync(id, cancellationToken);

        return details is null ? NotFound() : Ok(details);
    }
}
