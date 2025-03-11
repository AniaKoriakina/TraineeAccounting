using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class UpdateTraineeshipTraineesHandler : IRequestHandler<UpdateTraineeshipTraineesCommand, bool>
{
    private readonly ITraineeshipRepository _traineeshipRepository;

    public UpdateTraineeshipTraineesHandler(ITraineeshipRepository traineeshipRepository)
    {
        _traineeshipRepository = traineeshipRepository;
    }

    public async Task<bool> Handle(UpdateTraineeshipTraineesCommand request, CancellationToken cancellationToken)
    {
        if (request.TraineeIds == null || !request.TraineeIds.Any())
        {
            return false;
        }

        return await _traineeshipRepository.UpdateTraineesTraineeshipAsync(request.TraineeshipId, request.TraineeIds);
    }
}