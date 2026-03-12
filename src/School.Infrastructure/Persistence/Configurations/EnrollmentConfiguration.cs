using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EnrollmentDate)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt);

        builder.HasIndex(e => new { e.StudentId, e.AcademicYearId })
            .IsUnique();

        builder.HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ClassRoom)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.ClassRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.AcademicYear)
            .WithMany(a => a.Enrollments)
            .HasForeignKey(e => e.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}