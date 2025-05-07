using Api.Extensions;
using Application.service;
using Application.Service;
using Infrastructure.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller.v1;

[ApiController]
[Route("api/[controller]/[action]")]
[ApiVersion("1")]
[Produces("application/json")]
public class EmployeeController : ControllerBase {
  private readonly ILogger<EmployeeController> _logger;
  private readonly EmployeeService _service;

  public EmployeeController(ILogger<EmployeeController> logger, EmployeeService service) {
    _logger = logger;
    _service = service;
  }

  [HttpGet]
  [ProducesResponseType(typeof(employee[]), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetEmployees(CancellationToken ct) {
    var results = await _service.FindAll(ct);
    _logger.LogResults(typeof(employee), results);
    return Ok(results);
  }

  [HttpGet]
  [ProducesResponseType(typeof(employee), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetEmployee(short employeeId, CancellationToken ct) {
    var result = await _service.FindById(employeeId, ct);
    if (result is null) {
      _logger.LogInformation("Customer {CustomerId} not found", employeeId);

      return NotFound(new ProblemDetails {
        Title = "Employee not found",
        Status = StatusCodes.Status404NotFound,
        Detail = $"Not employee with id „{employeeId}“ exists",
      });
    }
    return Ok(result);
  }
}
