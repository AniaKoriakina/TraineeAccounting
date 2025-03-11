using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project> GetByNameAsync(string projectName);
    Task<Project?> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllAsync();
    Task AddAsync(Project project);
    Task<bool> DeleteAsync(Project project);
    Task<bool> UpdateTraineesProjectAsync(int projectId, List<int> traineesIds);
    Task<int> GetTraineesCountAsync(int traineesId);
}