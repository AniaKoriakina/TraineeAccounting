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

    public async Task AddAsync(Traineeship traineeship)
    {
        await _context.Traineeships.AddAsync(traineeship);
        await _context.SaveChangesAsync();
    }
}