using FluentValidation;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Validators;

public class UpdateProjectTraineesValidator : AbstractValidator<UpdateProjectTraineesCommand>
{
    private readonly ITraineeRepository _traineeRepository;
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectTraineesValidator(ITraineeRepository traineeRepository, IProjectRepository projectRepository)
    {
        _traineeRepository = traineeRepository;
        _projectRepository = projectRepository;
        
        RuleFor(x => x.ProjectId)
            .GreaterThan(0)
            .WithMessage("ID стажировки должен быть больше 0");
        
        RuleFor(x => x.TraineeIds)
            .NotNull()
            .NotEmpty()
            .MustAsync(async (ids, ct) =>
                ids != null && await _traineeRepository.AreTraineesExist(ids))
            .WithMessage("Некоторые стажеры не найдены");
    }
}