using Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller.v1;

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
    _logger.LogInformation("Found Employees: {@Result}", result);
    return Ok(result);
  }

  [HttpGet]
  public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken) {
    var result = await _service.FindCustomers(cancellationToken);
    _logger.LogInformation("Found Customers: {@Result}", result);
    return Ok(result);
  }

  [HttpGet]
  public async Task<IActionResult> GetCustomer(string name, CancellationToken cancellationToken) {
    var result = new object[0];
    await Task.Delay(1);
    _logger.LogInformation("Found Customer: {@Result}", result);
    return Ok(result);
  }

  [HttpGet]
  public async Task<IActionResult> GetEmployee(string name, CancellationToken cancellationToken) {
    var result = new object[0];
    await Task.Delay(1);
    _logger.LogInformation("Found Employee: {@Result}", result);
    return Ok(result);
  }
}
