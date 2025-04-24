using Infrastructure.Entity;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace Application.Service;
public class NorthwindService {

  private readonly ILogger<NorthwindService> _logger;
  private readonly NorthwindRepository _repository;

  public NorthwindService(ILogger<NorthwindService> logger, NorthwindRepository repository) {
    _logger = logger;
    _repository = repository;
  }

  public async Task<employee[]> FindEmployees(CancellationToken ct) {
    _logger.LogTrace("");
    return await _repository.FetchEmployees(ct);
  }

  public async Task<customer[]> FindCustomers(CancellationToken ct) {
    _logger.LogTrace("");
    return await _repository.FetchCustomers(ct);
  }

  
  public async Task<employee?> FindEmployeeById(short id, CancellationToken ct) {
    _logger.LogTrace("");
    return await _repository.FetchEmployeeById(id, ct);
  }

  public async Task<customer?> FindCustomerById(string id, CancellationToken ct) {
    _logger.LogTrace("");
    return await _repository.FetchCustomerById(id, ct);
  }
}
