using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var traineeCount = await _projectRepository.GetTraineesCountAsync(request.ProjectId);
        if (traineeCount > 0)
        {
            throw new ApplicationException($"Project cannot be deleted. Count dependent trainees: {traineeCount}");
        }
        
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null) return false;
        
        await _projectRepository.DeleteAsync(project);
        return true;
    }
}