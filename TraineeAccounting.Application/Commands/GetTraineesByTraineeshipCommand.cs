using MediatR;
using TraineeAccounting.Application.Dtos;

namespace TraineeAccounting.Application.Commands;

public class GetTraineesByTraineeshipCommand : IRequest<List<TraineeDto>>
{
    public int TraineeshipId { get; set; }
}