using MediatR;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Application.Commands;

public class GetTraineesByProjectCommand : IRequest<List<TraineeDto>>
{
    public int ProjectId { get; set; }
}