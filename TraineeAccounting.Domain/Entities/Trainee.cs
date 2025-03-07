using System.ComponentModel.DataAnnotations;

namespace TraineeAccounting.Domain.Entities;

public class Trainee
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string Gender { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    [Required]
    public DateOnly DateOfBirth { get; set; }
    
    [Required]
    public Traineeship Traineeship { get; set; }
    
    [Required]
    public Project Project { get; set; }
    
    protected Trainee() {}
    
    public Trainee(
        string firstName,
        string lastName,
        string gender,
        string email,
        string phoneNumber,
        DateOnly dateOfBirth,
        Traineeship traineeship,
        Project project)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Email = email;
        PhoneNumber = phoneNumber;
        DateOfBirth = dateOfBirth;
        Traineeship = traineeship ?? throw new ArgumentNullException(nameof(traineeship));
        Project = project ?? throw new ArgumentNullException(nameof(project));
    }
}