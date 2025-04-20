using Infrastructure.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.repository;
public class DefaultRepository {
  private readonly ILogger<DefaultRepository> _logger;
  private readonly IDbContextFactory<NorthwindContext> _factory;

  public DefaultRepository(ILogger<DefaultRepository> logger,  IDbContextFactory<NorthwindContext> factory) {
    _logger = logger;
    _factory = factory;
  }

  public async Task<object[]> FetchSometing(int something, CancellationToken cancellationToken) {
    var ctx = await _factory.CreateDbContextAsync(cancellationToken);
    return [];
  }
}
