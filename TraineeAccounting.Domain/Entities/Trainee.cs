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
    
    public string? PhoneNumber { get; set; }
    
    [Required]
    public DateOnly DateOfBirth { get; set; }
    
    public int? TraineeshipId { get; set; }
    
    [Required]
    public Traineeship Traineeship { get; set; }
    
    public int? ProjectId { get; set; }
    
    [Required]
    public Project Project { get; set; }
    
    protected Trainee() {}
    
    public Trainee(
        string firstName,
        string lastName,
        string gender,
        string email,
        string? phoneNumber,
        DateOnly dateOfBirth,
        Traineeship traineeship,
        Project project)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Email = email;
        PhoneNumber = phoneNumber ?? PhoneNumber;
        DateOfBirth = dateOfBirth;
        Traineeship = traineeship ?? throw new ArgumentNullException(nameof(traineeship));
        Project = project ?? throw new ArgumentNullException(nameof(project));
    }
    
    public void UpdatePersonalInfo(
        string? firstName = null,
        string? lastName = null,
        string? gender = null,
        string? email = null,
        string? phoneNumber = null,
        DateOnly? dateOfBirth = null)
    {
        FirstName = firstName ?? FirstName;
        LastName = lastName ?? LastName;
        Gender = gender ?? Gender;
        Email = email ?? Email;
        PhoneNumber = phoneNumber ?? PhoneNumber;
        DateOfBirth = dateOfBirth ?? DateOfBirth;
    }
    
    public void ChangeTraineeship(Traineeship newTraineeship)
    {
        Traineeship = newTraineeship;
        TraineeshipId = newTraineeship?.TraineeshipId;
    }
    
    public void ChangeProject(Project newProject)
    {
        Project = newProject;
        ProjectId = newProject?.ProjectId;
    }
}