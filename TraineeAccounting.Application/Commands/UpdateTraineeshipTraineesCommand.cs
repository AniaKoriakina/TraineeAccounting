using MediatR;

namespace TraineeAccounting.Application.Commands;

public class UpdateTraineeshipTraineesCommand : IRequest<bool>
{
    public int TraineeshipId { get; set; }
    public List<int> TraineeIds { get; set; } = new();
}