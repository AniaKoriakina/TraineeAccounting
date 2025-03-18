using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, int>
{
    private readonly IProjectRepository _projectRepository;

    public CreateProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectName = request.Name;
        var existingProject = await _projectRepository.GetByNameAsync(projectName);
        if (existingProject != null)
        {
            throw new Exception($"Traineeship with name {projectName} already exists");
        }
        Project project ;
        project = new Project(projectName);
        await _projectRepository.AddAsync(project);
        return project.ProjectId;
    }
}