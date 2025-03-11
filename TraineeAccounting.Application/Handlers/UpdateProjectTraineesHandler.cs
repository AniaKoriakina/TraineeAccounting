using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class UpdateProjectTraineesHandler : IRequestHandler<UpdateProjectTraineesCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectTraineesHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(UpdateProjectTraineesCommand request, CancellationToken cancellationToken)
    {
        if (request.TraineeIds == null || !request.TraineeIds.Any())
        {
            return false;
        }

        return await _projectRepository.UpdateTraineesProjectAsync(request.ProjectId, request.TraineeIds);
    }
}