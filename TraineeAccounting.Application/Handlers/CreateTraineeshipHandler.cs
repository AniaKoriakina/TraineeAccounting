using System.Data;
using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class CreateTraineeshipHandler : IRequestHandler<CreateTraineeshipCommand, int>
{
    private readonly ITraineeshipRepository _traineeshipRepository;

    public CreateTraineeshipHandler(ITraineeshipRepository traineeshipRepository)
    {
        _traineeshipRepository = traineeshipRepository;
    }

    public async Task<int> Handle(CreateTraineeshipCommand request, CancellationToken cancellationToken)
    {
        var traineeshipName = request.TraineeshipName;
        var existingTraineeship = await _traineeshipRepository.GetByNameAsync(traineeshipName);
        if (existingTraineeship != null)
        {
            throw new DataException($"Traineeship with name {traineeshipName} already exists");
        }
        Traineeship traineeship;
        traineeship = new Traineeship(traineeshipName);
        await _traineeshipRepository.AddAsync(traineeship);
        return traineeship.TraineeshipId;
    }
}