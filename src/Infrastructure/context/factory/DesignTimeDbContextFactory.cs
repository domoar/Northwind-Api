using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.context;
using Microsoft.EntityFrameworkCore.Design;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<NorthwindContext> {
  public NorthwindContext CreateDbContext(string[] args) {
    var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();

    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=YourSecretPassword;Database=northwind;Ssl Mode=Require;Trust Server Certificate=true");

    var loggerFactory = LoggerFactory.Create(builder =>
    {

    });

    var logger = loggerFactory.CreateLogger<NorthwindContext>();

    return new NorthwindContext(logger, optionsBuilder.Options);
  }
}
