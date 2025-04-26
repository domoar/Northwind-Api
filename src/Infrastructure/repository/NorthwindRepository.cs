using Infrastructure.Context;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository;
public class NorthwindRepository {
  private readonly ILogger<NorthwindRepository> _logger;
  private readonly IDbContextFactory<NorthwindContext> _factory;

  public NorthwindRepository(ILogger<NorthwindRepository> logger, IDbContextFactory<NorthwindContext> factory) {
    _logger = logger;
    _factory = factory;
  }

  public async Task<employee[]> FetchEmployees(CancellationToken ct) {
    var ctx = await _factory.CreateDbContextAsync(ct);
    var result = await ctx.employees.ToArrayAsync(ct);
    _logger.LogTrace("Found {Count} {Type}", result.Length, typeof(employee)); //TODO adjust log from non reflection
    return result;
  }

  public async Task<customer[]> FetchCustomers(CancellationToken ct) {
    var ctx = await _factory.CreateDbContextAsync(ct);
    var result = await ctx.customers.ToArrayAsync(ct);
    _logger.LogTrace("Found {Count} {Type}", result.Length, typeof(customer)); //TODO adjust log from non reflection
    return result;
  }

  public async Task<employee?> FetchEmployeeById(short id, CancellationToken ct) {
    var ctx = await _factory.CreateDbContextAsync(ct);
    var result = await ctx.employees
      .AsNoTracking()
      .SingleOrDefaultAsync(e => e.employee_id == id, ct);

    return result;
  }

  public async Task<customer?> FetchCustomerById(string id, CancellationToken ct) {
    var ctx = await _factory.CreateDbContextAsync(ct);
    var result = await ctx.customers
      .AsNoTracking()
      .SingleOrDefaultAsync(c => c.customer_id == id, ct);

    return result;
  }

  public async Task<T?> GetById<T>(int id, CancellationToken ct) where T : class {
    var ctx = await _factory.CreateDbContextAsync(ct);
    return await ctx.Set<T>().FindAsync([id], cancellationToken: ct);
  }

  public async Task Add<T>(T entity, CancellationToken ct) where T : class {
    var ctx = await _factory.CreateDbContextAsync(ct);
    await ctx.Set<T>().AddAsync(entity, ct);
  }

  public async Task Update<T>(T entity, CancellationToken ct) where T : class {
    var ctx = await _factory.CreateDbContextAsync(ct);
    ctx.Set<T>().Update(entity);
  }

  public async Task Delete<T>(T entity, CancellationToken ct) where T : class {
    var ctx = await _factory.CreateDbContextAsync(ct);
    ctx.Set<T>().Remove(entity);
  }

  public async Task SaveChangesAsync(CancellationToken ct) {
    var ctx = await _factory.CreateDbContextAsync(ct);
    await ctx.SaveChangesAsync(ct);
  }
}
