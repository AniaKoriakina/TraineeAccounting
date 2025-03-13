using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Models;

namespace TraineeAccounting.Domain.Interfaces;

public interface ITraineeRepository
{
    Task<Trainee> GetByIdAsync(int id);
    Task<IEnumerable<Trainee>> GetTraineesAsync();
    Task AddAsync(Trainee trainee);
    
    Task<bool> DeleteAsync(Trainee trainee);
    
    Task UpdateAsync(Trainee trainee);
    
    Task<bool> IsEmailExistsAsync(string email);
    
    Task<bool> IsPhoneNumberExistAsync(string phone);
    Task<bool> AreTraineesExist(List<int> traineeIds);
    Task<PagedResult<Trainee>> GetPaginatedAsync(SearchAndSortRequest request, CancellationToken cancellationToken);
}