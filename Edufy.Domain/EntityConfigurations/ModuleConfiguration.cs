using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasIndex(m => new { m.ProgramId, m.Order })
            .IsUnique();

        builder.HasOne(m => m.Program)
            .WithMany(p => p.Modules)
            .HasForeignKey(m => m.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
