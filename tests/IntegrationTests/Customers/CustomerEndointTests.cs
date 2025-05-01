using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using static IntegrationTests.NorthwindApiFactory;

namespace IntegrationTests.Customers;

[Collection("ApiFactory collection")]
public class CustomerEndointTests {
  private readonly HttpClient _client;

  public CustomerEndointTests(NorthwindApiFactory factory) {
    _client = factory.CreateClient();
  }

  [Fact(DisplayName = "GET /api/Customer/GetCustomers returns 200 OK")]
  [Trait("category", "customer")]
  public async Task GetCustomers_ReturnsOk() {
    // Arrange
    const string url = "api/Customer/GetCustomers?";

    // Act
    var response = await _client.GetAsync(url);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact(DisplayName = "GET /api/Customer/GetCustomerById returns 200 OK")]
  [Trait("category", "customer")]
  public async Task GetCustomer_ReturnsOk_ForValidId() {
    // Arrange
    const string url = "api/Customer/GetCustomer?customerid=ALFKI";

    // Act
    var response = await _client.GetAsync(url);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }
}
