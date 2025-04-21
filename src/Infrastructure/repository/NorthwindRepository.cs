using Infrastructure.context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.repository;
public class NorthwindRepository {
  private readonly ILogger<NorthwindRepository> _logger;
  private readonly IDbContextFactory<NorthwindContext> _factory;

  public NorthwindRepository(ILogger<NorthwindRepository> logger,  IDbContextFactory<NorthwindContext> factory) {
    _logger = logger;
    _factory = factory;
  }

  public async Task<employee[]> FetchEmployees(CancellationToken cancellationToken) {
    var ctx = await _factory.CreateDbContextAsync(cancellationToken);
    var result = await ctx.employees.ToArrayAsync(cancellationToken);
    _logger.LogTrace("Found {Count} {Type}", result.Length, typeof(employee)); //TODO adjust log from non reflection
    return result;
  }

  public async Task<customer[]> FetchCustomers(CancellationToken cancellationToken) {
    var ctx = await _factory.CreateDbContextAsync(cancellationToken);
    var result = await ctx.customers.ToArrayAsync(cancellationToken);
    _logger.LogTrace("Found {Count} {Type}", result.Length, typeof(customer)); //TODO adjust log from non reflection
    return result;
  }
}
