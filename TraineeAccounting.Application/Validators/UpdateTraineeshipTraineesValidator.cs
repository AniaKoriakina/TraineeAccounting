using FluentValidation;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Validators;

public class UpdateTraineeshipTraineesValidator : AbstractValidator<UpdateTraineeshipTraineesCommand>
{
    private readonly ITraineeRepository _traineeRepository;
    private readonly ITraineeshipRepository _traineeshipRepository;

    public UpdateTraineeshipTraineesValidator(ITraineeRepository traineeRepository, ITraineeshipRepository traineeshipRepository)
    {
        _traineeRepository = traineeRepository;
        _traineeshipRepository = traineeshipRepository;
        
        RuleFor(x => x.TraineeshipId)
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