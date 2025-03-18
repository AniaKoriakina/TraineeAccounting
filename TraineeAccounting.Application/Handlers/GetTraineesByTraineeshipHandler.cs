using MediatR;
using TraineeAccounting.Application.Commands;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Interfaces;

namespace TraineeAccounting.Application.Handlers;

public class GetTraineesByTraineeshipHandler : IRequestHandler<GetTraineesByTraineeshipCommand, List<TraineeDto>>
{
    private readonly ITraineeshipRepository _traineeshipRepository;

    public GetTraineesByTraineeshipHandler(ITraineeshipRepository traineeshipRepository)
    {
        _traineeshipRepository = traineeshipRepository;
    }

    public async Task<List<TraineeDto>> Handle(GetTraineesByTraineeshipCommand request, CancellationToken cancellationToken)
    {
        var trainees = await _traineeshipRepository.GetTraineesByTraineeshipAsync(request.TraineeshipId);
        return trainees.Select(t => new TraineeDto
        {
            TraineeId = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Gender = t.Gender,
            Email = t.Email,
            PhoneNumber = t.PhoneNumber,
            DateOfBirth = t.DateOfBirth,
            ProjectName = t.Project.Name,
            TraineeshipName = t.Traineeship.Name,
        }).ToList();
    }
}