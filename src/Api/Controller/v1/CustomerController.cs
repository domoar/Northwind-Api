using Api.Extension;
using Api.Request.Parameters;
using Application.service;
using Application.Service;
using Infrastructure.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller.v1;

[ApiController]
[Route("api/[controller]/[action]")]
[ApiVersion("1")]
[Produces("application/json")]
public class CustomerController : ControllerBase {
  private readonly ILogger<CustomerController> _logger;
  private readonly CustomerService _service;

  public CustomerController(ILogger<CustomerController> logger, CustomerService service) {
    _logger = logger;
    _service = service;
  }

  [HttpGet]
  [ProducesResponseType(typeof(customer[]), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetCustomers(CancellationToken ct) {
    var results = await _service.FindAll(ct);
    _logger.LogResults(typeof(employee), results);
    return Ok(results);
  }

  [HttpGet]
  [ProducesResponseType(typeof(customer), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetCustomer([FromQuery] CustomerRequest req, CancellationToken ct) {
    var result = await _service.FindById(req.CustomerID, ct);
    if (result is null) {
      _logger.LogInformation("Customer {CustomerId} not found", req.CustomerID);
      return NotFound(new ProblemDetails {
        Title = "Customer not found",
        Status = StatusCodes.Status404NotFound,
        Detail = $"Not customer with id „{req.CustomerID}“ exists",
      });
    }
    return Ok(result);
  }


}
