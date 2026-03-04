using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public class ProgramConfiguration : IEntityTypeConfiguration<Program>
{
    public void Configure(EntityTypeBuilder<Program> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(p => p.About)
            .HasMaxLength(3000);

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasOne(p => p.Academy)
            .WithMany(a => a.Programs)
            .HasForeignKey(p => p.AcademyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Instructor)
            .WithMany(i => i.Programs)
            .HasForeignKey(p => p.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
