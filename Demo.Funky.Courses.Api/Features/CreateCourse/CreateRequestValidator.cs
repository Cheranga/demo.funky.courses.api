using FluentValidation;

namespace Demo.Funky.Courses.Api.Features.CreateCourse;

public class CreateRequestValidator : AbstractValidator<Request>
{
    public CreateRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("name is required");
        RuleFor(x => x.EnrollmentDate).GreaterThan(DateTime.UtcNow).WithMessage("enrollment date must be in the future");
    }
}