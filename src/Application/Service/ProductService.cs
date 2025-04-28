using System.Reflection;
using Infrastructure.Entity;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Service;
public class ProductService {

  private readonly ILogger<ProductService> _logger;
  private readonly NorthwindRepository _repository;

  public ProductService(ILogger<ProductService> logger, NorthwindRepository repository) {
    _logger = logger;
    _repository = repository;
  }

  public async Task<product> Create(product product, CancellationToken ct) {
    await _repository.Add(product, ct);
    await _repository.SaveChangesAsync(ct);
    _logger.LogInformation("Product created: {ProductId}", product.product_id);
    return product;
  }

  public async Task<product?> Update(int id, product product, CancellationToken ct) {
    var existing = await _repository.GetById<product>(id, ct);
    if (existing == null) {
      _logger.LogWarning("Product not found for update: {ProductId}", id);
      return null;
    }

    existing.product_name = product.product_name;
    existing.unit_price = product.unit_price;
    existing.units_in_stock = product.units_in_stock;
    existing.category_id = product.category_id;

    //TODO other properties and mapping

    await _repository.Update(existing, ct);
    await _repository.SaveChangesAsync(ct);
    return existing;
  }

  public async Task<product?> PatchAsync(int id, Dictionary<string, object> updates, CancellationToken ct) {
    var product = await _repository.GetById<product>(id, ct);
    if (product == null) {
      _logger.LogWarning("Product not found for patch: {ProductId}", id);
      return null;
    }

    var type = typeof(product);
    foreach (var kvp in updates) {
      var prop = type.GetProperty(kvp.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
      if (prop != null && prop.CanWrite) {
        object? value = Convert.ChangeType(kvp.Value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        prop.SetValue(product, value);
      }
    }

    await _repository.Update(product, ct);
    await _repository.SaveChangesAsync(ct);
    return product;
  }

  public async Task<bool> DeleteAsync(int id, CancellationToken ct) {
    var product = await _repository.GetById<product>(id, ct);
    if (product == null) {
      _logger.LogWarning("Product not found for delete: {ProductId}", id);
      return false;
    }

    await _repository.Delete(product, ct);
    await _repository.SaveChangesAsync(ct);
    _logger.LogInformation("Product deleted: {ProductId}", id);
    return true;
  }
}
