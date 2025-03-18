using MediatR;
using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Application.Commands;

public class CreateTraineeshipCommand : IRequest<int>
{
    public int TraineeshipId { get; set; }
    public string Name { get; set; }
}