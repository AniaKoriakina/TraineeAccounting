using System.Text.RegularExpressions;
using FluentValidation;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Validators;

public class UpdateTraineeValidator : AbstractValidator<UpdateTraineeCommand>
{
    private readonly ITraineeRepository _traineeRepository;
    public UpdateTraineeValidator(ITraineeRepository traineeRepository)
    {
        _traineeRepository = traineeRepository;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.FirstName))
            .WithMessage("Имя должно быть указано");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage("Фамилия должна быть указана");
        RuleFor(x => x.Gender)
            .NotEmpty()
            .Must(x => x == "Женский" || x == "Мужской")
            .When(x => !string.IsNullOrEmpty(x.Gender))
            .WithMessage("Требуется корретный пол (Женский или Мужской)");
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Укажите корректный email");
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (command, email, ct) =>
                await _traineeRepository.IsEmailUnique(command.TraineeId, email))
            .WithMessage("Этот email уже используется");
        RuleFor(x => x.PhoneNumber)
            .Matches(new Regex(@"^\+7\d{10}$")).WithMessage("Требуется действительный номер телефона")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .MustAsync(async (command, phoneNumber, ct) =>
                await _traineeRepository.IsPhoneNumberUnique(command.TraineeId, phoneNumber))
            .WithMessage("Этот номер телефона уже используется");
    }
    
    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return !(await _traineeRepository.IsEmailExistsAsync(email));
    }
    
    private async Task<bool> BeUniquePhone(string phone, CancellationToken cancellationToken)
    {
        return !(await _traineeRepository.IsPhoneNumberExistAsync(phone));
    }
}