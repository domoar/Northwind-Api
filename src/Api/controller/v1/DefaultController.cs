using Api.dtos;
using Application.service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.controller.v1;

[ApiController]
[Route("api/[controller]/[action]")]
[ApiVersion("1")]
[Produces("application/json")]
public class NorthwindController : ControllerBase {
  private readonly ILogger<NorthwindController> _logger;
  private readonly NorthwindService _service;

  public NorthwindController(ILogger<NorthwindController> logger, NorthwindService service) {
      _logger = logger;
      _service = service;
  }

  [HttpGet]
  public async Task<IActionResult> GetEmployees(CancellationToken cancellationToken) {
    var result = await _service.FindEmployees(cancellationToken);
    _logger.LogDebug("Employees: {Result}", result);
    return Ok(result);
  }

  [HttpGet]
  public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken) {
    var result = await _service.FindCustomers(cancellationToken);
    _logger.LogDebug("Customers: {Result}", result);
    return Ok(result);
  }
}
