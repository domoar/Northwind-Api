using System.Net;
using FluentAssertions;
using static IntegrationTests.NorthwindApiFactory;

namespace IntegrationTests;

[Collection("ApiFactory collection")]
public class BaseIntegrationTest {

  private readonly HttpClient _client;

  [Fact(DisplayName = "Sanity test")]
  [Trait("category", "employee")]

  public void True_Should_Be_True() {
    // Arrange
    Boolean boolValue;

    // Act
    boolValue = true;

    // Assert
    boolValue.Should().BeTrue("Expected the value to be true.");
  }

  [Theory(DisplayName = "Sanity test")]
  [InlineData(true)]
  public void True_Should_Be_True_ForData(bool inlineData) {
    // Arrange
    Boolean boolValue;

    // Act 
    boolValue = inlineData;

    // Assert
    boolValue.Should().BeTrue("Expected the value to be true.");
  }

  public BaseIntegrationTest(NorthwindApiFactory apiFactory) {
    _client = apiFactory.CreateClient();
  }
}
