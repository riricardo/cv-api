using Microsoft.AspNetCore.Mvc;
using cv_api.Models;
using cv_api.Queries;
using cv_api.Repositories;

namespace cv_api.Controllers;

public class ResumesController : CrudControllerBase<Resume>
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

        var resume = await queryService.GetResumeDetailsAsync(id, cancellationToken);

        return resume is null ? NotFound() : Ok(resume);
    }

    [HttpGet("by-link/{linkId}")]
    public async Task<IActionResult> GetByLink(
        string linkId,
        [FromServices] QueryService queryService,
        CancellationToken cancellationToken
    )
    {
        var resume = await queryService.GetResumeByLinkAsync(linkId, cancellationToken);

        return resume is null ? NotFound() : Ok(resume);
    }
}
