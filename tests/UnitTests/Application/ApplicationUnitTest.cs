using FluentAssertions;

namespace UnitTests.Application;

[Trait("category", "application")]
public class BaseUnitTest {
  [Fact]
  public void True_Should_Be_True() {
    // Arrange
    Boolean boolValue;

    // Act
    boolValue = true;

    // Assert
    boolValue.Should().BeTrue("Expected the value to be true.");
  }

  [Theory]
  [InlineData(true)]
  public void True_Should_Be_True_ForData(bool inlineData) {
    // Arrange
    Boolean boolValue;

    // Act 
    boolValue = inlineData;

    // Assert
    boolValue.Should().BeTrue("Expected the value to be true.");
  }
}
