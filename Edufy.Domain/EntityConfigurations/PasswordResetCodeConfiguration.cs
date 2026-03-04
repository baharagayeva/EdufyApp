using Edufy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edufy.Domain.EntityConfigurations;

public sealed class PasswordResetCodeConfiguration: IEntityTypeConfiguration<PasswordResetCode>
{
    public void Configure(EntityTypeBuilder<PasswordResetCode> b)
    {
        b.ToTable("PasswordResetCodes");

        b.Property(x => x.Email).HasMaxLength(200).IsRequired();
        b.Property(x => x.CodeHash).HasMaxLength(200).IsRequired();

        b.HasIndex(x => x.Email);
        b.HasIndex(x => new { x.Email, x.ExpiresAt });
    }
}