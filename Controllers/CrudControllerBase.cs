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
        if (!DocumentId.IsValid(id))
        {
            return BadRequest(new { message = "Id must be a valid GUID." });
        }

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
        string id;

        try
        {
            id = DocumentId.CreateIfMissing(document);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }

        if (await repository.ExistsByIdAsync(id, cancellationToken))
        {
            return Conflict(new CreateDocumentResult(id, "ignored"));
        }

        DocumentTimestamps.SetCreated(document, DateTime.UtcNow);

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
        if (!DocumentId.IsValid(id))
        {
            return BadRequest(new { message = "Id must be a valid GUID." });
        }

        string documentId;

        try
        {
            documentId = DocumentId.Get(document);
        }
        catch (InvalidOperationException)
        {
            DocumentId.Set(document, id);
            documentId = id;
        }

        if (!string.Equals(id, documentId, StringComparison.Ordinal))
        {
            return BadRequest(new { message = "Route id must match document Id." });
        }

        if (!DocumentId.IsValid(documentId))
        {
            return BadRequest(new { message = "Document Id must be a valid GUID." });
        }

        var currentDocument = await repository.GetByIdAsync(id, cancellationToken);

        if (currentDocument is null)
        {
            return NotFound();
        }

        DocumentTimestamps.SetUpdated(document, currentDocument, DateTime.UtcNow);

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
        if (!DocumentId.IsValid(id))
        {
            return BadRequest(new { message = "Id must be a valid GUID." });
        }

        var deleted = await repository.DeleteAsync(id, cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}
