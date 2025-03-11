using MediatR;

namespace TraineeAccounting.Application.Commands;

public class UpdateProjectTraineesCommand : IRequest<bool>
{
    public int ProjectId { get; set; }
    public List<int> TraineeIds { get; set; } = new();
}