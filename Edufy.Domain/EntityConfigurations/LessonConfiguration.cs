using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(l => l.VideoUrl)
            .HasMaxLength(512);

        builder.HasIndex(l => new { l.ModuleId, l.Order })
            .IsUnique();

        builder.HasOne(l => l.Module)
            .WithMany(m => m.Lessons)
            .HasForeignKey(l => l.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}