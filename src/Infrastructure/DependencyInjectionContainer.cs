using Infrastructure.Context;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure;
public static class DependencyInjectionContainer {
  const int POOL_SIZE = 1024;

  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration) {

    services.AddScoped<NorthwindRepository>();

    var connection = configuration.GetConnectionString("DefaultConnection");

    services.AddPooledDbContextFactory<NorthwindContext>((sp, options) => {
      options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

      if (environment.IsDevelopment()) {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
      }
      var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
      options.UseLoggerFactory(loggerFactory);

      options.LogTo(
        message => loggerFactory.CreateLogger("EFCore"),
        [DbLoggerCategory.Database.Command.Name],
        LogLevel.Information,
        DbContextLoggerOptions.SingleLine
      );

      options.UseNpgsql(connection, sqlOptions => {
        sqlOptions.EnableRetryOnFailure(
          maxRetryCount: 10,
          maxRetryDelay: TimeSpan.FromSeconds(10),
          errorCodesToAdd: []
        );
      });

    },
    POOL_SIZE);

    return services;
  }
}
