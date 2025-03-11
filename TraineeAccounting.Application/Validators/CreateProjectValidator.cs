using FluentValidation;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Validators;

public class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    private readonly IProjectRepository _projectRepository;

    public CreateProjectValidator(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
        
        RuleFor(x => x.ProjectName)
            .NotNull().WithMessage("Название проекта обязательно для заполнения")
            .NotEmpty().WithMessage("Название проекта не может быть пустым")
            .MustAsync(async (name, cancellationToken) => 
            {
                var existingProject = await _projectRepository.GetByNameAsync(name);
                return existingProject == null; 
            })
            .WithMessage("Проект с таким названием уже существует");
    }
}