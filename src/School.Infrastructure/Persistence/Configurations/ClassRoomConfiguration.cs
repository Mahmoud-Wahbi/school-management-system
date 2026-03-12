using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class ClassRoomConfiguration : IEntityTypeConfiguration<ClassRoom>
{
    public void Configure(EntityTypeBuilder<ClassRoom> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Section)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Capacity)
            .IsRequired();

        builder.Property(c => c.GradeLevel)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        builder.HasIndex(c => new { c.AcademicYearId, c.GradeLevel, c.Section })
            .IsUnique();

        builder.HasOne(c => c.AcademicYear)
            .WithMany(a => a.ClassRooms)
            .HasForeignKey(c => c.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.HomeroomTeacher)
            .WithMany(t => t.HomeroomClassRooms)
            .HasForeignKey(c => c.HomeroomTeacherId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}