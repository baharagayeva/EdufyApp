using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public class AcademyConfiguration : IEntityTypeConfiguration<Academy>
{
    public void Configure(EntityTypeBuilder<Academy> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.LogoUrl)
            .HasMaxLength(512);

        builder.Property(a => a.About)
            .HasMaxLength(2000);

        builder.Property(a => a.GraduationRate)
            .HasPrecision(5, 2);
    }
}