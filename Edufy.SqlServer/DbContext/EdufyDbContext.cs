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
    
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<Lesson> Lessons { get; set; }
    public required DbSet<InstructorProfile> InstructorProfiles { get; set; }
    public required DbSet<CourseModule> CourseModules { get; set; }
    public required DbSet<CourseApplication> CourseApplications { get; set; }
    public required DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email).IsUnique();

        modelBuilder.Entity<CourseModule>()
            .HasIndex(x => new { x.CourseId, x.Order }).IsUnique();

        modelBuilder.Entity<Lesson>()
            .HasIndex(x => new { x.CourseModuleId, x.Order }).IsUnique();
        
        base.OnModelCreating(modelBuilder);
    }
}