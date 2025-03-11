using Microsoft.EntityFrameworkCore;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
using TraineeAccounting.Infrastructure.Data;

namespace TraineeAccounting.Infrastructure.Repositories;

public class TraineeRepository : ITraineeRepository
{
    private readonly ApplicationDbContext _context;

    public TraineeRepository(ApplicationDbContext context)
    {
        _context = context;
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