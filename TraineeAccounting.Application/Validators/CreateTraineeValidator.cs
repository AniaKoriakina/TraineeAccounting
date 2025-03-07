using System.Text.RegularExpressions;
using FluentValidation;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Validators;

public class CreateTraineeValidator : AbstractValidator<CreateTraineeCommand>
{
    private readonly ITraineeRepository _traineeRepository;
    public CreateTraineeValidator(ITraineeRepository traineeRepository)
    {
        _traineeRepository = traineeRepository;
        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .WithMessage("Название проекта обязательно к заполнению");
        RuleFor(x => x.TraineeshipName)
            .NotEmpty()
            .WithMessage("Название стажировки обязательно к заполнению");
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("Имя обязательно к заполнению");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Фамилия обязательна к заполнению");
        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("Необходимо выбрать пол")
            .Must(x => x == "Женский" || x == "Мужской")
            .WithMessage("Требуется корретный пол (Женский или Мужской)");
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Дата рождения обязательна к заполнению");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email адрес обязателен к заполнению")
            .EmailAddress().WithMessage("Требуется действительный email адрес")
            .MustAsync(BeUniqueEmail).WithMessage("Email адрес уже существует");
        RuleFor(x => x.PhoneNumber)
            .Matches(new Regex(@"^\+7\d{10}$")).WithMessage("Требуется действительный номер телефона")
            .MustAsync(BeUniquePhone).WithMessage("Номер телефона уже существует");
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