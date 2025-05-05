using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using static IntegrationTests.NorthwindApiFactory;

namespace IntegrationTests.Employees;

[Collection("ApiFactory collection")]
public class EmployeeEndpointTests {
  private readonly HttpClient _client;

  public EmployeeEndpointTests(NorthwindApiFactory factory) {
    _client = factory.CreateClient();
  }

  [Fact(DisplayName = "GET /api/Employee/GetEmployees returns 200 OK")]
  [Trait("category", "employee")]
  public async Task GetEmployees_ReturnsOk() {
    // Arrange
    const string url = "api/Employee/GetEmployees?";

    // Act
    var response = await _client.GetAsync(url);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact(DisplayName = "GET /api/Employee/GetEmployeeById returns 200 OK")]
  [Trait("category", "employee")]
  public async Task GetEmployee_ReturnsOk_ForValidId() {
    // Arrange
    const string url = "api/Employee/GetEmployee?employeeId=1";

    // Act
    var response = await _client.GetAsync(url);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }
}
