using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Context.Factory;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NorthwindContext> {
  public NorthwindContext CreateDbContext(string[] args) {
    var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();

    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=p;Database=northwind;Search Path=northwind;Ssl Mode=Require;Trust Server Certificate=true"); //TODO from cfg

    var loggerFactory = LoggerFactory.Create(builder => {

    });

    var logger = loggerFactory.CreateLogger<NorthwindContext>();

    return new NorthwindContext(logger, optionsBuilder.Options);
  }
}
