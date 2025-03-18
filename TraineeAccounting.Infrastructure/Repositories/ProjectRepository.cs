using Microsoft.EntityFrameworkCore;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Domain.Models;
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
    
    public async Task<PagedResult<Project>> GetPaginatedAsync(SearchAndSortRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Projects.Include(ts =>ts.Trainees).AsQueryable();
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(t => t.Name.ToLower().Contains(request.Search.ToLower()));
        }

        switch (request.Sort?.ToLower())
        {
            case "name":
                query = request.SortDirection ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name);
                break;
            case "trainee_count":
                query = request.SortDirection ? query.OrderBy(t => t.Trainees.Count) : query.OrderByDescending(t => t.Trainees.Count);
                break;
            default:
                query = query.OrderBy(t => t.Name);
                break;
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);
        return new PagedResult<Project>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = request.PageSize,
            PageIndex =request.PageIndex
        };
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

    public async Task<List<Trainee>> GetTraineesByProjectAsync(int projectId)
    {
        return await _context.Trainees
            .Where(t => t.Project.ProjectId == projectId)
            .Include(t => t.Project)
            .Include(t => t.Traineeship)
            .ToListAsync();
    }
}