using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Domain.Interfaces;

public interface ITraineeRepository
{
    Task<Trainee> GetByIdAsync(int id);
    Task<IEnumerable<Trainee>> GetTraineesAsync();
    Task AddAsync(Trainee trainee);
    
    Task DeleteAsync(Trainee trainee);
    
    Task<bool> IsEmailExistsAsync(string email);
    
    Task<bool> IsPhoneNumberExistAsync(string phone);
}