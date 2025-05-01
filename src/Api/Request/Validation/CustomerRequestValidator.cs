using Api.Request.Parameters;
using FluentValidation;

namespace Api.Request.Validation;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest> {
    public const int MAX_ID_LENGTH = 55;

    public CustomerRequestValidator() {
        RuleFor(x => x.CustomerID)
            .Cascade(CascadeMode.Stop)
            .NotNull()
                .WithErrorCode("CustomerID.NotNull")
                .WithMessage("Customer Id cannot be null")
            .NotEmpty()
                .WithErrorCode("CustomerID.NotEmpty")
                .WithMessage("Customer Id cannot be empty")
            .Must(id => string.IsNullOrWhiteSpace(id) == false)
                .WithErrorCode("CustomerID.WhitespaceOnly")
                .WithMessage("Customer Id cannot be whitespace")
            .MaximumLength(MAX_ID_LENGTH)
                .WithErrorCode("CustomerID.TooLong")
                .WithMessage($"Customer Id cannot be longer can {MAX_ID_LENGTH}");
    }
}
