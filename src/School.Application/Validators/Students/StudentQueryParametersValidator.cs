using FluentValidation;
using School.Application.DTOs.Students;

namespace School.Application.Validators.Students;

public class StudentQueryParametersValidator : AbstractValidator<StudentQueryParameters>
{
    public StudentQueryParametersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize must not exceed 100.");

        RuleFor(x => x.Name)
        .MaximumLength(100)
        .When(x => !string.IsNullOrWhiteSpace(x.Name));

    }
}