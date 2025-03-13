using Microsoft.EntityFrameworkCore;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Domain.Models;
using TraineeAccounting.Infrastructure.Data;

namespace TraineeAccounting.Infrastructure.Repositories;

public class TraineeshipRepository : ITraineeshipRepository
{
    private readonly ApplicationDbContext _context;

    public TraineeshipRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Traineeship>> GetAllAsync()
    {
        return await _context.Traineeships.ToListAsync();
    }

    public async Task<Traineeship?> GetByNameAsync(string name)
    {
        return await _context.Traineeships
            .Where(t => t.Name.ToLower() == name.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<Traineeship?> GetByIdAsync(int id)
    {
        return await _context.Traineeships.FindAsync(id);
    }

    public async Task AddAsync(Traineeship traineeship)
    {
        await _context.Traineeships.AddAsync(traineeship);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetTraineesCountAsync(int traineesipsId)
    {
        return await _context.Trainees.CountAsync(t => t.Traineeship.TraineeshipId == traineesipsId);
    }

    public async Task<bool> UpdateTraineesTraineeshipAsync(int traineeshipId, List<int> traineeIds)
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
            trainee.TraineeshipId = traineeshipId;
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

    public async Task<PagedResult<Traineeship>> GetPaginatedAsync(SearchAndSortRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Traineeships.Include(ts =>ts.Trainees).AsQueryable();
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
        return new PagedResult<Traineeship>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = request.PageSize,
            PageIndex =request.PageIndex
        };
    }

    public async Task<bool> DeleteAsync(Traineeship traineeship)
    {
        _context.Traineeships.Remove(traineeship);
        await _context.SaveChangesAsync();
        return true;
    }
}