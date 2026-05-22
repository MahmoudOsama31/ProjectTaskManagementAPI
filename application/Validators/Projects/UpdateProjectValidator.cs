using FluentValidation;
using ProjectTaskManagement.DTOs.Projects;

namespace ProjectTaskManagement.Validators.Projects;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .When(x => x.Description != null);
    }
}
