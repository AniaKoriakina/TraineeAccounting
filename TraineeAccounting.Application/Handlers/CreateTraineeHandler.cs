using FluentValidation;
using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class CreateTraineeHandler : IRequestHandler<CreateTraineeCommand, int>
{
    private readonly ITraineeRepository _traineeRepository;
    private readonly ITraineeshipRepository _traineeshipRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IValidator<CreateTraineeCommand> _validator;

    public CreateTraineeHandler(IValidator<CreateTraineeCommand> validator, ITraineeRepository traineeRepository, ITraineeshipRepository traineeshipRepository, IProjectRepository projectRepository)
    {
        _traineeRepository = traineeRepository;
        _traineeshipRepository = traineeshipRepository;
        _projectRepository = projectRepository;
        _validator = validator;
    }

    public async Task<int> Handle(CreateTraineeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var traineeshipName = request.TraineeshipName;
        var existingTraineeship = await _traineeshipRepository.GetByNameAsync(traineeshipName);
        Traineeship traineeship;
        if (existingTraineeship != null)
        {
            traineeship = existingTraineeship;
        }
        else
        {
            traineeship = new Traineeship(traineeshipName);
            await _traineeshipRepository.AddAsync(traineeship);
        }
        
        var projectName = request.ProjectName;
        var existingProject = await _projectRepository.GetByNameAsync(projectName);
        Project project;
        if (existingProject != null)
        {
            project = existingProject;
        }
        else
        {
            project = new Project(projectName);
            await _projectRepository.AddAsync(project);
        }

        var trainee = new Trainee(
            request.FirstName,
            request.LastName,
            request.Gender,
            request.Email,
            request?.PhoneNumber,
            request.DateOfBirth,
            traineeship,
            project
        );
        await _traineeRepository.AddAsync(trainee);
        return trainee.Id;
    }
}