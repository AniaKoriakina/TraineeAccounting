using Microsoft.EntityFrameworkCore;
using TraineeAccounting.Domain.Entities;
using TraineeAccounting.Domain.Interfaces;
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
    

    public async Task<bool> DeleteAsync(Traineeship traineeship)
    {
        _context.Traineeships.Remove(traineeship);
        await _context.SaveChangesAsync();
        return true;
    }
}