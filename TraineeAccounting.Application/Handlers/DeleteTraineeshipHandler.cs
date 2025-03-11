using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class DeleteTraineeshipHandler : IRequestHandler<DeleteTraineeshipCommand, bool>
{
    private readonly ITraineeshipRepository _traineeshipRepository;
    private readonly ITraineeRepository _traineeRepository;

    public DeleteTraineeshipHandler(
        ITraineeshipRepository traineeshipRepository,
        ITraineeRepository traineeRepository)
    {
        _traineeshipRepository = traineeshipRepository;
        _traineeRepository = traineeRepository;
    }

    public async Task<bool> Handle(DeleteTraineeshipCommand request, CancellationToken cancellationToken)
    {
        var traineeCount = await _traineeshipRepository.GetTraineesCountAsync(request.TraineeshipId);
        if (traineeCount > 0)
        {
            throw new ApplicationException($"Traineeship cannot be deleted. Count dependent trainees: {traineeCount}");
        }
        
        var traineeship = await _traineeshipRepository.GetByIdAsync(request.TraineeshipId);
        if (traineeship == null) return false;

        await _traineeshipRepository.DeleteAsync(traineeship);
        return true;
    }
}