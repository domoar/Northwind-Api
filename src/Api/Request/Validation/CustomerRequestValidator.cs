using Api.Request.Parameters;
using FluentValidation;

namespace Api.Request.Validation;

public class CustomerRequestValidator<T> : AbstractValidator<T> where T : CustomerRequest {
  public const int MAX_ID_LENGTH = 55;

  //TODO use child rules for better structure
  public CustomerRequestValidator() {
    RuleFor(c => c.CustomerID)
        .NotNull()
        .WithMessage("Customer ID cannot be null")
        .NotEmpty()
        .WithMessage("Customer ID cannot be empty.")
        .Must(c => c!.Trim().Length > 0)
        .WithMessage("Customer ID cannot contain only whitespace.")
        .MaximumLength(MAX_ID_LENGTH)
        .WithMessage($"Customer ID cannot exceed {MAX_ID_LENGTH} characters.");

  }
}
