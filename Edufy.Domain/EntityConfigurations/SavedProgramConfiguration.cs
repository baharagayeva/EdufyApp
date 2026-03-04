using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public class SavedProgramConfiguration : IEntityTypeConfiguration<SavedProgram>
{
    public void Configure(EntityTypeBuilder<SavedProgram> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SavedAt);

        // One user cannot save the same program twice
        builder.HasIndex(s => new { s.UserId, s.ProgramId })
            .IsUnique();

        builder.HasOne(s => s.User)
            .WithMany(u => u.SavedPrograms)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Program)
            .WithMany(p => p.SavedPrograms)
            .HasForeignKey(s => s.ProgramId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
