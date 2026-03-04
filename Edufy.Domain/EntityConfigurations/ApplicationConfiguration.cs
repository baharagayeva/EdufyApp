using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.AppliedAt);

        // One user cannot apply to the same program twice
        builder.HasIndex(a => new { a.UserId, a.ProgramId })
            .IsUnique();

        builder.HasOne(a => a.User)
            .WithMany(u => u.Applications)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Program)
            .WithMany(p => p.Applications)
            .HasForeignKey(a => a.ProgramId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
