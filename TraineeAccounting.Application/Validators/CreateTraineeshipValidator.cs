using FluentValidation;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Validators;

public class CreateTraineeshipValidator : AbstractValidator<CreateTraineeshipCommand>
{
    private readonly ITraineeshipRepository _repository;

    public CreateTraineeshipValidator(ITraineeshipRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Name)
            .NotNull().WithMessage("Название стажировки обязательно для заполнения")
            .NotEmpty().WithMessage("Название стажировки не может быть пустым")
            .MustAsync(async (name, cancellationToken) => 
            {
                var existingTraineeship = await _repository.GetByNameAsync(name);
                return existingTraineeship == null; 
            })
            .WithMessage("Стажировка с таким названием уже существует");
        
    }
}