using MediatR;

namespace TraineeAccounting.Application.Commands;

public class DeleteTraineeCommand : IRequest<bool>
{
    public int TraineeId { get; set; }
}