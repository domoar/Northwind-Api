using Infrastructure.Entity;
using Infrastructure.Repository;
using Microsoft.Extensions.Logging;

namespace Application.service;
public class EmployeeService {
  private readonly ILogger<EmployeeService> _logger;
  private readonly NorthwindRepository _repository;

  public EmployeeService(ILogger<EmployeeService> logger, NorthwindRepository repository) {
    _logger = logger;
    _repository = repository;
  }

  public async Task<employee?> FindById(short id, CancellationToken ct) {
    _logger.LogTrace("");
    return await _repository.FetchEmployeeById(id, ct);
  }

  public async Task<employee[]> FindAll(CancellationToken ct) {
    _logger.LogTrace("");
    return await _repository.FetchEmployees(ct);
  }
}
