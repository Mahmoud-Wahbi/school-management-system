using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.Property(t => t.PhoneNumber)
            .HasMaxLength(30);

        builder.Property(t => t.Specialization)
            .HasMaxLength(150);

        builder.Property(t => t.NationalId)
            .HasMaxLength(50);

        builder.Property(t => t.Address)
            .HasMaxLength(500);

        builder.Property(t => t.IsActive)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt);

        builder.HasIndex(t => t.UserId)
            .IsUnique()
            .HasFilter("[UserId] IS NOT NULL");
    }
}