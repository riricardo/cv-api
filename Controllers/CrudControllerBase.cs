using Microsoft.AspNetCore.Mvc;
using cv_api.Api;
using cv_api.Auth;
using cv_api.Repositories;

namespace cv_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class CrudControllerBase<T> : ControllerBase
    where T : class
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] IRepository<T> repository,
        CancellationToken cancellationToken
    )
    {
        var documents = await repository.GetAllAsync(cancellationToken);

        return Ok(documents);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        string id,
        [FromServices] IRepository<T> repository,
        CancellationToken cancellationToken
    )
    {
        var document = await repository.GetByIdAsync(id, cancellationToken);

        return document is null ? NotFound() : Ok(document);
    }

    [HttpPost]
    [RequireMasterKey]
    public async Task<IActionResult> Create(
        T document,
        [FromServices] IRepository<T> repository,
        CancellationToken cancellationToken
    )
    {
        var id = DocumentId.Get(document);

        if (await repository.ExistsByIdAsync(id, cancellationToken))
        {
            return Conflict(new CreateDocumentResult(id, "ignored"));
        }

        await repository.CreateAsync(document, cancellationToken);

        return Created($"{Request.Path}/{id}", new CreateDocumentResult(id, "inserted"));
    }

    [HttpPut("{id}")]
    [RequireMasterKey]
    public async Task<IActionResult> Update(
        string id,
        T document,
        [FromServices] IRepository<T> repository,
        CancellationToken cancellationToken
    )
    {
        var documentId = DocumentId.Get(document);

        if (!string.Equals(id, documentId, StringComparison.Ordinal))
        {
            return BadRequest(new { message = "Route id must match document Id." });
        }

        var updated = await repository.UpdateAsync(id, document, cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [RequireMasterKey]
    public async Task<IActionResult> Delete(
        string id,
        [FromServices] IRepository<T> repository,
        CancellationToken cancellationToken
    )
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}
