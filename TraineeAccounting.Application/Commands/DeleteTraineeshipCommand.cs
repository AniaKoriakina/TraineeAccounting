using MediatR;

namespace TraineeAccounting.Application.Commands;

public class DeleteTraineeshipCommand : IRequest<bool>
{
    public int TraineeshipId { get; set; }
}