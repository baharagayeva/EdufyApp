using Edufy.Domain.Entities;
using Edufy.Domain.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Edufy.SqlServer.DbContext;

public class EdufyDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public EdufyDbContext(DbContextOptions<EdufyDbContext> options) : base(options)
    {
    } 
    
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Academy> Academies => Set<Academy>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Program> Programs => Set<Program>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<SavedLesson> SavedLessons => Set<SavedLesson>();
    public DbSet<PasswordResetCode> PasswordResetCodes => Set<PasswordResetCode>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("gp_edufy");
        modelBuilder.ApplyConfiguration(new AcademyConfiguration());
        modelBuilder.ApplyConfiguration(new InstructorConfiguration());
        modelBuilder.ApplyConfiguration(new ProgramConfiguration());
        modelBuilder.ApplyConfiguration(new ModuleConfiguration());
        modelBuilder.ApplyConfiguration(new LessonConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
        modelBuilder.ApplyConfiguration(new SavedProgramConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new PasswordResetCodeConfiguration());
        
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email).IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}