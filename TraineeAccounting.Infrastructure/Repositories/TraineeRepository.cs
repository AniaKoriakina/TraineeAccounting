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

    public async Task DeleteAsync(Trainee trainee)
    {
        _context.Trainees.Remove(trainee);
        await _context.SaveChangesAsync();
    }
    
}