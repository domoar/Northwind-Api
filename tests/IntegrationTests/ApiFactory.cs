using Api;
using ConsoleTables;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Testcontainers.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests;
public class NorthwindApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime {

  private readonly ILogger<NorthwindApiFactory> _logger;
  private readonly PostgreSqlContainer _databaseContainer;
  public NorthwindApiFactory() {

    using var loggerFactory = LoggerFactory.Create(builder => {
      builder
        .AddConsole()
        .AddSimpleConsole(options => {
          options.SingleLine = true;
          options.TimestampFormat = "hh:mm:ss ";
        })
        .AddDebug()
        .SetMinimumLevel(LogLevel.Debug);
    });

    _logger = loggerFactory.CreateLogger<NorthwindApiFactory>();

    var scriptsDir = AppContext.BaseDirectory!;

    _logger.LogInformation("Using SQL directory: {SqlDir}", scriptsDir);

    _databaseContainer = new PostgreSqlBuilder()
      .WithImage("postgres:16-alpine")
      .WithName("northwind-postgres-testcontainer")
      .WithDatabase("postgres")
      .WithUsername("postgres")
      .WithPassword("yourStrong(!)Password")
      .WithCleanUp(true)
      .WithPortBinding(5432, 5432)
      .WithBindMount(
          Path.Combine(scriptsDir, "create-db-northwind.sql"),
          "/scripts/create-db-northwind.sql"
        )
      .WithBindMount(
          Path.Combine(scriptsDir, "create-schema-northwind.sql"),
          "/scripts/create-schema-northwind.sql"
        )
      .WithBindMount(
          Path.Combine(scriptsDir, "northwind.sql"),
          "/scripts/northwind.sql"
        )
      .Build();
  }

  public async Task InitializeAsync() {
    await _databaseContainer.StartAsync();

    try {
      await RunInContainer(_databaseContainer, "mkdir", "-p", "/var/lib/postgresql/tablespace/northwind");
      await RunInContainer(_databaseContainer, "chown", "postgres:postgres", "/var/lib/postgresql/tablespace/northwind");
      await RunInContainer(_databaseContainer,
          "psql", "-v", "ON_ERROR_STOP=1", "-U", "postgres", "-d", "postgres",
          "-f", "/scripts/create-db-northwind.sql"
      );
      await RunInContainer(_databaseContainer,
          "psql", "-v", "ON_ERROR_STOP=1", "-U", "postgres", "-d", "northwind",
          "-f", "/scripts/create-schema-northwind.sql"
      );
      await RunInContainer(_databaseContainer,
          "psql", "-v", "ON_ERROR_STOP=1", "-U", "postgres", "-d", "northwind",
          "-f", "/scripts/northwind.sql"
      );

      var csb = new NpgsqlConnectionStringBuilder(_databaseContainer.GetConnectionString()) {
        Database = "northwind"
      };
      await using var conn = new NpgsqlConnection(csb.ConnectionString);
      await conn.OpenAsync();

      _logger.LogDebug("Tables in northwind schema:");
      const string listTablesSql = @"
         SELECT table_name
           FROM information_schema.tables
          WHERE table_schema = 'northwind'
       ORDER BY table_name;
     ";
      await using (var tableCmd = new NpgsqlCommand(listTablesSql, conn))
      await using (var tableReader = await tableCmd.ExecuteReaderAsync()) {
        while (await tableReader.ReadAsync())
          _logger.LogDebug(" {Content}", tableReader.GetString(0));
      }
    }
    catch (Exception ex) {
      _logger.LogError(ex, "An error occurred while initializing or seeding the Postgres container");
    }
  }

  async Task IAsyncLifetime.DisposeAsync() {
    await Task.Delay(1);
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder) {
    var raw = _databaseContainer.GetConnectionString();
    var csb = new NpgsqlConnectionStringBuilder(raw) {
      Database = "northwind",
      SslMode = SslMode.Disable
    };

    var integrationTestConnection = csb.ToString();

    builder.ConfigureServices((context, services) => {

      services.RemoveAll<IDbContextFactory<NorthwindContext>>();
      services.RemoveAll(typeof(DbContextOptions<NorthwindContext>));
      services.RemoveAll(typeof(NorthwindContext));

      services.AddPooledDbContextFactory<NorthwindContext>((sp, options) =>
      {
        var env = sp.GetRequiredService<IHostEnvironment>();
        var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

        options
          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
          .UseLoggerFactory(loggerFactory)
          .UseNpgsql(integrationTestConnection, sqlOptions => {
            sqlOptions.EnableRetryOnFailure(
              maxRetryCount: 10,
              maxRetryDelay: TimeSpan.FromSeconds(10),
              errorCodesToAdd: Array.Empty<string>()
            );
          });

        if (env.IsDevelopment()) {
          options.EnableDetailedErrors();
          options.EnableSensitiveDataLogging();
        }
      }, poolSize: 1024);
    });

    _logger.LogDebug("Testcontainer configured with connection: {Connection}", integrationTestConnection);
  }

  static async Task RunInContainer(PostgreSqlContainer container, params string[] cmd) {
    var result = await container.ExecAsync(cmd);
    if (result.ExitCode != 0) {
      throw new Exception($"Error running `{string.Join(" ", cmd)}` in container: {result.Stderr}");
    }
  }
}

[CollectionDefinition("ApiFactory collection")]
public class SharedApiFactoryFixtureCollection : ICollectionFixture<NorthwindApiFactory> { }
