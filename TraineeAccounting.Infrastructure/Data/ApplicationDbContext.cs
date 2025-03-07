using Microsoft.EntityFrameworkCore;
using TraineeAccounting.Domain.Entities;

namespace TraineeAccounting.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Trainee>? Trainees { get; set; }
    public DbSet<Project>? Projects { get; set; }
    public DbSet<Traineeship>? Traineeships { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=trainee;Username=postgres;Password=7825;");
        }
    }
}