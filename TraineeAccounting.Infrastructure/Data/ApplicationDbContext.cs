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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Trainee>()
            .HasOne(t => t.Traineeship)
            .WithMany(ts => ts.Trainees)
            .HasForeignKey(t => t.TraineeshipId)
            .OnDelete(DeleteBehavior.SetNull); 
        
        modelBuilder.Entity<Trainee>()
            .HasOne(t => t.Project)
            .WithMany(ps => ps.Trainees)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}