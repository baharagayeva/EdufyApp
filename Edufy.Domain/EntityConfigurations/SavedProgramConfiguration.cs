using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public class SavedProgramConfiguration : IEntityTypeConfiguration<SavedLesson>
{
    public void Configure(EntityTypeBuilder<SavedLesson> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SavedAt);

        // One user cannot save the same program twice
        builder.HasIndex(x => new { x.UserId, x.LessonId })
            .IsUnique();

        builder.HasOne(s => s.User)
            .WithMany(u => u.SavedLessons)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Lesson)
            .WithMany(p => p.SavedLessons)
            .HasForeignKey(s => s.LessonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
