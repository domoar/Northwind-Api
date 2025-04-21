using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Entities;
using Infrastructure.repository;
using Microsoft.Extensions.Logging;

namespace Application.service;
public class NorthwindService {

  private readonly ILogger<NorthwindService> _logger;
  private readonly NorthwindRepository _repository;

  public NorthwindService(ILogger<NorthwindService> logger, NorthwindRepository repository) {
    _logger = logger;
    _repository = repository;
  }

  public async Task<employee[]> FindEmployees(CancellationToken cancellationToken) {
    _logger.LogTrace("");
    return await _repository.FetchEmployees(cancellationToken);
  }

  public async Task<customer[]> FindCustomers(CancellationToken cancellationToken) {
    _logger.LogTrace("");
    return await _repository.FetchCustomers(cancellationToken);
  }
}
