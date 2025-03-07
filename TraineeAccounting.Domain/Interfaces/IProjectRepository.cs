using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project> GetByNameAsync(string projectName);
    Task<IEnumerable<Project>> GetAllAsync();
    Task AddAsync(Project project);
}