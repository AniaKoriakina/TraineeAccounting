using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class GetTraineesByProjectHandler : IRequestHandler<GetTraineesByProjectCommand, List<TraineeDto>>
{
    private readonly IProjectRepository _projectRepository;

    public GetTraineesByProjectHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<List<TraineeDto>> Handle(GetTraineesByProjectCommand request, CancellationToken cancellationToken)
    {
        var trainees = await _projectRepository.GetTraineesByProjectAsync(request.ProjectId);
        
        return trainees.Select(t => new TraineeDto()
        {
            TraineeId = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Gender = t.Gender,
            Email = t.Email,
            PhoneNumber = t.PhoneNumber,
            DateOfBirth = t.DateOfBirth,
            TraineeshipName = t.Traineeship.Name,
            ProjectName = t.Project.Name
        }).ToList();
    }
}
