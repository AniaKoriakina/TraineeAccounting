using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Domain.Interfaces;

public interface ITraineeshipRepository
{
    Task<Traineeship?> GetByNameAsync(string traineeshipName);
    Task AddAsync(Traineeship traineeship);
    Task<IEnumerable<Traineeship>> GetAllAsync();
}