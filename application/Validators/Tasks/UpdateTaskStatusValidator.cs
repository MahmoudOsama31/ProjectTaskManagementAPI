using FluentValidation;
using ProjectTaskManagement.DTOs.Tasks;

namespace ProjectTaskManagement.Validators.Tasks;

public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusDto>
{
    public UpdateTaskStatusValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}
