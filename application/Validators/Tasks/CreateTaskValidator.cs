using FluentValidation;
using ProjectTaskManagement.DTOs.Tasks;

namespace ProjectTaskManagement.Validators.Tasks;

public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .When(x => x.Description != null);

        RuleFor(x => x.Priority).IsInEnum();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
