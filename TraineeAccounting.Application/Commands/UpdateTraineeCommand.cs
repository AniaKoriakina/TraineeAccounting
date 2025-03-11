using MediatR;

namespace TraineeAccounting.Application.Commands;

public class UpdateTraineeCommand : IRequest<bool>
{
    public int TraineeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? TraineeshipName { get; set; }
    public string? ProjectName { get; set; }
}