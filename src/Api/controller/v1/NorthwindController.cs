using Application.Service;
using Infrastructure.Entity;
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
  [ProducesResponseType(typeof(employee[]), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetEmployees(CancellationToken ct) {
    var result = await _service.FindEmployees(ct);
    _logger.LogInformation("Found Employees: {@Result}", result);
    return Ok(result);
  }

  [HttpGet]
  [ProducesResponseType(typeof(customer[]), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetCustomers(CancellationToken ct) {
    var result = await _service.FindCustomers(ct);
    _logger.LogInformation("Found Customers: {@Result}", result);
    return Ok(result);
  }

  [HttpGet]
  [ProducesResponseType(typeof(employee), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetEmployee(short employeeId, CancellationToken ct) {
    var result = await _service.FindEmployeeById(employeeId, ct);
 if (result is null)
    {
        _logger.LogInformation("Customer {CustomerId} not found", employeeId);

        return NotFound(new ProblemDetails
        {
            Title  = "Employee not found",
            Status = StatusCodes.Status404NotFound,
            Detail = $"Not employee with id „{employeeId}“ exists"
        });
    }
    return Ok(result);
  }

  [HttpGet]
  [ProducesResponseType(typeof(customer), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetCustomer(string customerId, CancellationToken ct) {
    var result = await _service.FindCustomerById(customerId, ct);
    if (result is null)
    {
        _logger.LogInformation("Customer {CustomerId} not found", customerId);

        return NotFound(new ProblemDetails
        {
            Title  = "Customer not found",
            Status = StatusCodes.Status404NotFound,
            Detail = $"Not customer with id „{customerId}“ exists"
        });
    }
    return Ok(result);
  }
}
