using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TraineeAccounting.Client.Models;

public class Trainee
{
    [Required]
    [JsonPropertyName("traineeId")]
    public int TraineeId { get; set; }
    
    [Required]
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    
    [Required]
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    
    [Required]
    [JsonPropertyName("gender")]
    public string Gender { get; set; }
    
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; }
    
    [Required]
    [JsonPropertyName("dateOfBirth")]
    public DateOnly DateOfBirth { get; set; }
    
    [Required]
    [JsonPropertyName("traineeship")]
    public Traineeship Traineeship { get; set; }
    
    [Required]
    [JsonPropertyName("project")]
    public Project Project { get; set; }
}