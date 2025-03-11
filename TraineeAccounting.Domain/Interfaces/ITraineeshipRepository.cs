using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Domain.Interfaces;

public interface ITraineeshipRepository
{
    Task<Traineeship?> GetByNameAsync(string traineeshipName);
    Task<Traineeship?> GetByIdAsync(int id);
    Task AddAsync(Traineeship traineeship);
    Task<IEnumerable<Traineeship>> GetAllAsync();
    Task<int> GetTraineesCountAsync(int traineesId);
    Task<bool> UpdateTraineesTraineeshipAsync(int traineeshipId, List<int> traineesIds);
    Task<bool> DeleteAsync(Traineeship traineeship);
}