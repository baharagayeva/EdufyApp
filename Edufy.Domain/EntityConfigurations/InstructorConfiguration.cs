using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Specialization)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Bio)
            .HasMaxLength(1000);

        builder.Property(i => i.PhotoUrl)
            .HasMaxLength(512);

        builder.Property(i => i.PriceAzn)
            .HasPrecision(8, 2);

        builder.Property(i => i.LinkedInUrl)
            .HasMaxLength(512);
    }
}
