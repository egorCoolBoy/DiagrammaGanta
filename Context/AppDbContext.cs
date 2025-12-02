using Diagramma_Ganta.Model;
using Microsoft.EntityFrameworkCore;
using Task = Diagramma_Ganta.Model.Task;

namespace Diagramma_Ganta.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext>options):base(options){}
    public DbSet<User> Users { get; set; }
    public DbSet<Project>Projects { get; set; }
    public DbSet<Task>Tasks { get; set; }
    public DbSet<Subject>Subjects { get; set; }
    public DbSet<Team>Teams { get; set; }
    public DbSet<Session>Sessions { get; set; }
    public DbSet<TaskDependency>TaskDependencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(p =>
        {
            p.HasKey(p => p.Id);

            p.HasMany(p => p.Tasks).WithOne(t => t.Project).HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            p.HasOne(p => p.Team).WithOne().HasForeignKey<Team>(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            p.HasOne(p => p.Subject)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            p.HasOne(p => p.Owner).WithMany();
            

        });
        
        
        modelBuilder.Entity<Team>().HasMany(t => t.Users).WithMany(u => u.Teams);

        
        modelBuilder.Entity<Task>(t =>
        {
            t.HasOne(t => t.AuthorTask).WithMany();
        });

        modelBuilder.Entity<TaskDependency>(td =>
        {
            td.HasKey(td => new { td.TaskId, td.PredecessorId });
            
            td.HasOne(td => td.Predecessor).WithMany().HasForeignKey(td => td.PredecessorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            td.HasOne<Task>().WithMany(t => t.Dependencies).HasForeignKey(td => td.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<User>(u =>
        {
                u.HasIndex(u => u.Email)
                    .IsUnique();
        });

        modelBuilder.Entity<Session>(s =>
        {
                s.HasKey(s => s.Token);
                s.HasOne(s => s.User).WithMany()
                    .HasForeignKey(s => s.UserId);
        });
        
    }
}
