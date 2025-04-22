using Api;
using Infrastructure.context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace IntegrationTests;
public class NorthwindApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime {

private readonly ILogger<NorthwindApiFactory> _logger;
private readonly PostgreSqlContainer _databaseContainer;
public NorthwindApiFactory(){

  using var loggerFactory = LoggerFactory.Create(builder =>
  {
      builder
        .AddConsole()
        .AddDebug()
        .SetMinimumLevel(LogLevel.Information);
  });

  _logger = loggerFactory.CreateLogger<NorthwindApiFactory>();

_databaseContainer = new PostgreSqlBuilder()
    .WithImage("postgres:16.2-bookworm")
    .WithName("northwind-postgres-testcontainer")
    .WithCleanUp(true)
    .WithPortBinding(8888, 8888)
    .Build();
}

  public async Task InitializeAsync() {
    await _databaseContainer.StartAsync();


  }

  async Task IAsyncLifetime.DisposeAsync() {
    await _databaseContainer.StopAsync();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder) {
    var integrationTestConnection = _databaseContainer.GetConnectionString();
    _logger.LogDebug("Testcontainer: {ContainerName} is available at {Connection}", _databaseContainer.Name, integrationTestConnection);
    //TODO inject new connection to webhost without breaking the cfg
    base.ConfigureWebHost(builder);
  }
  [CollectionDefinition("ApiFactory context collection")]
  public class SharedApiFactoryFixtureCollection : ICollectionFixture<NorthwindApiFactory> { }
}
