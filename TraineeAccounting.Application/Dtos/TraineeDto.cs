using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Application.Dtos;

public class TraineeDto
{
    public int TraineeId { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Gender { get; set; }

    public string Email { get; set; }
    
    public string? PhoneNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public Traineeship Traineeship { get; set; }

    public Project Project { get; set; }
}