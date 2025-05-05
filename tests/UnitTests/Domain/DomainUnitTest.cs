using FluentAssertions;

namespace UnitTests.Domain;
[Trait("category", "domain")]
public class BaseUnitTest {
  [Fact(DisplayName = "Sanity test")]
  [Trait("category", "domain")]
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
  [Trait("category", "domain")]
  public void True_Should_Be_True_ForData(bool inlineData) {
    // Arrange
    Boolean boolValue;

    // Act 
    boolValue = inlineData;

    // Assert
    boolValue.Should().BeTrue("Expected the value to be true.");
  }
}
