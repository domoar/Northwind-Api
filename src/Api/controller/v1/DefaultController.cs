using Api.dtos;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.controller.v1;

[ApiController]
[Route("api/[controller]/[action]")]
[ApiVersion("1")]
[Produces("application/json")]
public class DefaultController : ControllerBase {
  private readonly ILogger<DefaultController> _logger;

  public DefaultController(ILogger<DefaultController> logger) {
      _logger = logger;
  }

  [HttpGet]
  public async Task<IActionResult> GetSomething([FromQuery] int something, CancellationToken cancellationToken) {
    await Task.Delay(1, cancellationToken);
    return Ok(something);
  }

  [HttpPost]
  public async Task<IActionResult> CreateSomething([FromBody] SomethingDto dto, CancellationToken cancellationToken) {
    var newId = new Random().Next(1, 1000);
    var created = new SomethingDto { Id = newId, Value = dto.Value };

    await Task.Delay(1, cancellationToken);

    _logger.LogInformation("Created new Something with ID {Id}", newId);

    return CreatedAtAction(
        nameof(CreateSomething),
        new { something = created.Value },
        created);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateSomething(
      int id,
      [FromBody] SomethingDto dto,
      CancellationToken cancellationToken) {
    _logger.LogInformation("Updating Something {Id} to Value {Value}", id, dto.Value);
    await Task.Delay(1, cancellationToken);

    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> PatchSomething(
      int id,
      [FromBody] JsonPatchDocument<SomethingDto> patchDoc,
      CancellationToken cancellationToken) {
    if (patchDoc == null)
      return BadRequest();

    var existing = new SomethingDto { Id = id, Value = 0 };

    patchDoc.ApplyTo(existing, error =>
    {
      var key = error.Operation?.path?.TrimStart('/');
      ModelState.AddModelError(key ?? string.Empty, error.ErrorMessage);
    });

    if (!ModelState.IsValid)
      return ValidationProblem(ModelState);

    _logger.LogInformation("Patched Something {Id}, new Value {Value}", id, existing.Value);
    await Task.Delay(1, cancellationToken);

    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteSomething(int id, CancellationToken cancellationToken) {
    _logger.LogInformation("Deleted Something with ID {Id}", id);
    await Task.Delay(1, cancellationToken);

    return NoContent();
  }
}
