using Microsoft.EntityFrameworkCore;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Infrastructure.Data;

namespace TraineeAccounting.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByNameAsync(string name)
    {
        return await _context.Projects.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }
    
}