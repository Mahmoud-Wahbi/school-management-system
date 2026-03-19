using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.EnrollmentNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.EnrollmentNumber)
            .IsUnique();

        builder.Property(s => s.Email)
            .HasMaxLength(256);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(30);

        builder.Property(s => s.Address)
            .HasMaxLength(500);

        builder.Property(s => s.NationalId)
            .HasMaxLength(50);

        builder.Property(s => s.IsActive)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt);

        builder.Property(s => s.OwnerUserId)
       .IsRequired();

        builder.HasOne(s => s.OwnerUser)
            .WithMany(u => u.OwnedStudents)
            .HasForeignKey(s => s.OwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}