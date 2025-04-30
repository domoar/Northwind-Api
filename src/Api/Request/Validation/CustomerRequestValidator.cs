using Api.Request.Parameters;
using FluentValidation;

namespace Api.Request.Validation;

public class CustomerRequestValidator<T> : AbstractValidator<T> where T : CustomerRequest {
  public const int MAX_ID_LENGTH = 55;

  public CustomerRequestValidator() {
    RuleFor(x => x.CustomerID)
        .NotNull()
            .WithErrorCode("CustomerID.NotNull")
        .NotEmpty()
            .WithErrorCode("CustomerID.NotEmpty")
        .Must(id => string.IsNullOrWhiteSpace(id) == false)
            .WithErrorCode("CustomerID.WhitespaceOnly")
        .MaximumLength(MAX_ID_LENGTH)
            .WithErrorCode("CustomerID.TooLong");
  }
}
