using MediatR;

namespace TraineeAccounting.Application.Commands;

public class CreateProjectCommand : IRequest<int>
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
}