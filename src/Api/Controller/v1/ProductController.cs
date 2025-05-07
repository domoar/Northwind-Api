using System.Reflection;
using System.Text.Json;
using Api.Extensions;
using Application.Service;
using Infrastructure.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Api.Controller.v1;

[ApiController]
[Route("api/[controller]/[action]")]
[ApiVersion("1")]
[Produces("application/json")]
public class ProductController : ControllerBase {
  private readonly ILogger<ProductController> _logger;
  private readonly ProductService _service;


  [HttpPost]
  public async Task<IActionResult> CreateProduct([FromBody] product product) {
    if (product == null) {
      return BadRequest("Product is null.");
    }
    await Task.Delay(1);
    return CreatedAtAction(nameof(CreateProduct), new { id = product.product_id }, product);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateProduct(int id, [FromBody] product product) {
    await Task.Delay(1);
    return NoContent();
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonElement updates) {
    await Task.Delay(1);
    return Ok();
  }

  // DELETE: api/Products/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProduct(int id) {
    await Task.Delay(1);
    return NoContent();
  }
}

