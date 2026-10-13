using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.FirstName)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(user => user.LastName)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(user => user.Email)
            .HasConversion(
                emailObj => emailObj.Value,
                emailStr => new EmailAddress(emailStr))
            .HasColumnName("Email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .IsRequired();

        builder.Property(user => user.Role)
            .IsRequired();
    }
}