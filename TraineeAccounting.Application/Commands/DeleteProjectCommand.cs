using MediatR;

namespace TraineeAccounting.Application.Commands;

public class DeleteProjectCommand : IRequest<bool>
{
    public int ProjectId { get; set; }
}