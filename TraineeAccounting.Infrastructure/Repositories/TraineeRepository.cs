using Microsoft.EntityFrameworkCore;
using TraineeAccounting.Application.Dtos;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Domain.Models;
using TraineeAccounting.Infrastructure.Data;

namespace TraineeAccounting.Infrastructure.Repositories;

public class TraineeRepository : ITraineeRepository
{
    private readonly ApplicationDbContext _context;

    public TraineeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<PagedResult<Trainee>> GetPaginatedAsync(SearchAndSortRequest request, CancellationToken cancellationToken)
    {
        var query = _context.Trainees
            .Include(t=>t.Project)
            .Include(t=>t.Traineeship)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(t => t.LastName.ToLower().Contains(request.Search.ToLower()) || 
                                     t.FirstName.ToLower().Contains(request.Search.ToLower()) ||
                                     t.Email.ToLower().Contains(request.Search.ToLower()) ||
                                     t.PhoneNumber.ToLower().Contains(request.Search.ToLower()) ||
                                     t.Traineeship.Name.ToLower().Contains(request.Search.ToLower()) ||
                                     t.Project.Name.ToLower().Contains(request.Search.ToLower()));
        }
        
        switch (request.Sort?.ToLower())
        {
            case "firstname":
                query = request.SortDirection ? query.OrderBy(t => t.FirstName) : query.OrderByDescending(t => t.FirstName);
                break;
            case "lastname":
                query = request.SortDirection ? query.OrderBy(t => t.LastName) : query.OrderByDescending(t => t.LastName);
                break;
            default:
                query = query.OrderBy(t => t.LastName);
                break;
        }
        
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Trainee>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex
        };
    }

    public async Task<IEnumerable<Trainee>> GetTraineesAsync()
    {
        return await _context.Trainees.Include(t => t.Traineeship).Include(p => p.Project).ToListAsync();
    }

    public async Task<Trainee?> GetByIdAsync(int id)
    {
        return await _context.Trainees.Include(t => t.Traineeship).Include(p => p.Project).FirstOrDefaultAsync(x => x.Id == id);
        
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _context.Trainees.AnyAsync(t => t.Email == email);
    }

    public async Task<bool> IsPhoneNumberExistAsync(string phoneNumber)
    {
        return await _context.Trainees.AnyAsync(t => t.PhoneNumber == phoneNumber);
    }

    public async Task AddAsync(Trainee trainee)
    {
        _context.Trainees.Add(trainee);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Trainee trainee)
    {
        _context.Trainees.Remove(trainee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task UpdateAsync(Trainee trainee)
    {
        _context.Trainees.Update(trainee);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> AreTraineesExist(List<int> traineeIds)
    {
        if (traineeIds == null || traineeIds.Count == 0) return false;

        var existingIds = await _context.Trainees
            .Where(t => traineeIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        return existingIds.Count == traineeIds.Count;
    }
    
}