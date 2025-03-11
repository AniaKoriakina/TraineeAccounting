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
    
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }
    
    public async Task<int> GetTraineesCountAsync(int projectId)
    {
        return await _context.Trainees.CountAsync(p => p.Project.ProjectId == projectId);
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTraineesProjectAsync(int projectId, List<int> traineeIds)
    {
        if (traineeIds == null || !traineeIds.Any())
        {
            return false;
        }
        
        var trainees = await _context.Trainees
            .Where(t => traineeIds.Contains(t.Id))
            .ToListAsync();

        if (trainees == null || !trainees.Any())
        {
            return false;
        }
        
        foreach (var trainee in trainees)
        {
            trainee.ProjectId = projectId;
        }

        try
        {
            _context.Trainees.UpdateRange(trainees);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving changes: {ex.Message}");
            return false;
        }
    }
}