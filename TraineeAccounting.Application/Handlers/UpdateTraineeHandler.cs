using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class UpdateTraineeHandler : IRequestHandler<UpdateTraineeCommand, bool>
{
    private readonly ITraineeRepository _traineeRepository;
    private readonly ITraineeshipRepository _traineeshipRepository;
    private readonly IProjectRepository _projectRepository;

    public UpdateTraineeHandler(
        ITraineeRepository traineeRepository,
        ITraineeshipRepository traineeshipRepository,
        IProjectRepository projectRepository)
    {
        _traineeRepository = traineeRepository;
        _traineeshipRepository = traineeshipRepository;
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(UpdateTraineeCommand request, CancellationToken cancellationToken)
    {
        var trainee = await _traineeRepository.GetByIdAsync(request.TraineeId);
        if (trainee == null)
        {
            Console.WriteLine($"Стажер с ID {request.TraineeId} не найден");
            return false;
        }

        if (!string.IsNullOrEmpty(request.FirstName) || 
            !string.IsNullOrEmpty(request.LastName) || 
            !string.IsNullOrEmpty(request.Gender) || 
            !string.IsNullOrEmpty(request.Email) || 
            request.PhoneNumber != null || 
            request.DateOfBirth.HasValue)
        {
            trainee.UpdatePersonalInfo(
                request.FirstName,
                request.LastName,
                request.Gender,
                request.Email,
                request.PhoneNumber,
                request.DateOfBirth);
        }

        if (!string.IsNullOrEmpty(request.TraineeshipName) && request.TraineeshipName != trainee.Traineeship?.Name)
        {
            var newTraineeship = await _traineeshipRepository.GetByNameAsync(request.TraineeshipName);
            if (newTraineeship == null)
            {
                newTraineeship = new Traineeship(request.TraineeshipName);
                await _traineeshipRepository.AddAsync(newTraineeship);
            }

            trainee.ChangeTraineeship(newTraineeship);
        }
        
        if (!string.IsNullOrEmpty(request.ProjectName) && request.ProjectName != trainee.Project?.Name)
        {
            var newProject = await _projectRepository.GetByNameAsync(request.ProjectName);
            if (newProject == null)
            {
                newProject = new Project(request.ProjectName);
                await _projectRepository.AddAsync(newProject);
            }

            trainee.ChangeProject(newProject);
        }
        
        try
        {
            await _traineeRepository.UpdateAsync(trainee);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении стажера: {ex.Message}");
            return false;
        }
    }
}