using Infrastructure.Entity;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace Application.service;
public class CustomerService {
  private readonly ILogger<CustomerService> _logger;
  private readonly NorthwindRepository _repository;

  public CustomerService(ILogger<CustomerService> logger, NorthwindRepository repository) {
    _logger = logger;
    _repository = repository;
  }

  public async Task<customer?> FindById(string id, CancellationToken ct) {
    _logger.LogTrace("");
    return await _repository.FetchCustomerById(id, ct);
  }

  public async Task<customer[]> FindAll(CancellationToken ct) {
    _logger.LogTrace("");
    return await _repository.FetchCustomers(ct);
  }
}
