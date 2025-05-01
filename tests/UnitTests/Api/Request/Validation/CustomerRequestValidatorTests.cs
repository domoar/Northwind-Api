using Api.Request.Parameters;
using Api.Request.Validation;
using FluentValidation.TestHelper;

namespace UnitTests.Api.Request.Validation;

[Trait("category", "api")]
public class CustomerRequestValidatorTests {
  private readonly CustomerRequestValidator _validator;

  public CustomerRequestValidatorTests() {
    _validator = new CustomerRequestValidator();
  }

  public static int MaxIdLength => CustomerRequestValidator.MAX_ID_LENGTH;

  [Fact]
  public void Should_Have_Error_When_CustomerId_Is_Null() {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    var parameters = new CustomerRequest { CustomerID = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    var result = _validator.TestValidate(parameters);

    result.ShouldHaveValidationErrorFor(p => p.CustomerID)
          .WithErrorCode("CustomerID.NotNull");
  }

  [Fact]
  public void Should_Have_Error_When_CustomerId_Is_Empty() {
    var parameters = new CustomerRequest { CustomerID = "" };

    var result = _validator.TestValidate(parameters);

    result.ShouldHaveValidationErrorFor(p => p.CustomerID)
          .WithErrorCode("CustomerID.NotEmpty");
  }

  [Fact]
  public void Should_Have_Error_When_CustomerId_Is_Only_Whitespace() {
    var parameters = new CustomerRequest { CustomerID = "   " };

    var result = _validator.TestValidate(parameters);

    result.ShouldHaveValidationErrorFor(p => p.CustomerID)
          .WithErrorCode("CustomerID.WhitespaceOnly");
  }

  [Fact]
  public void Should_Have_Error_When_CustomerId_Is_Too_Long() {
    var parameters = new CustomerRequest { CustomerID = new string('x', MaxIdLength + 1) };

    var result = _validator.TestValidate(parameters);

    result.ShouldHaveValidationErrorFor(p => p.CustomerID)
          .WithErrorCode("CustomerID.TooLong");
  }

  [Fact]
  public void Should_Not_Have_Errors_When_CustomerId_Is_Valid() {
    var parameters = new CustomerRequest { CustomerID = "ABCD123" };

    var result = _validator.TestValidate(parameters);

    result.ShouldNotHaveValidationErrorFor(p => p.CustomerID);
  }
}
