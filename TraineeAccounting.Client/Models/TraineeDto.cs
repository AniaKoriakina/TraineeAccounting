using System.ComponentModel.DataAnnotations;

namespace TraineeAccounting.Client.Models;

public class TraineeDto
{
    public int TraineeId { get; set; }
    
    [Required(ErrorMessage = "Имя должно быть указано")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Фамилия должна быть указана")]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Пол должен быть указан")]
    public string Gender { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "Укажите корректный email")]
    [Required(ErrorMessage = "Email должен быть указан")]
    public string? Email { get; set; }
    
    [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Требуется действительный номер телефона (начинается с +7)")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Дата рождения должна быть указана")]
    public DateOnly DateOfBirth { get; set; }
    public string TraineeshipName { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    
    public bool IsAlreadyAdded { get; set; } = false;
}