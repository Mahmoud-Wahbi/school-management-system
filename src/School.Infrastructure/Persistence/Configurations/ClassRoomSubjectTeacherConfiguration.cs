using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entities;

namespace School.Infrastructure.Persistence.Configurations;

public class ClassRoomSubjectTeacherConfiguration : IEntityTypeConfiguration<ClassRoomSubjectTeacher>
{
    public void Configure(EntityTypeBuilder<ClassRoomSubjectTeacher> builder)
    {
        builder.HasKey(cst => cst.Id);

        builder.Property(cst => cst.IsActive)
            .IsRequired();

        builder.Property(cst => cst.CreatedAt)
            .IsRequired();

        builder.Property(cst => cst.UpdatedAt);

        builder.HasIndex(cst => new { cst.ClassRoomId, cst.SubjectId, cst.AcademicYearId })
            .IsUnique();

        builder.HasOne(cst => cst.ClassRoom)
            .WithMany(c => c.ClassRoomSubjectTeachers)
            .HasForeignKey(cst => cst.ClassRoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cst => cst.Subject)
            .WithMany(s => s.ClassRoomSubjectTeachers)
            .HasForeignKey(cst => cst.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cst => cst.Teacher)
            .WithMany(t => t.ClassRoomSubjectTeachers)
            .HasForeignKey(cst => cst.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cst => cst.AcademicYear)
            .WithMany(a => a.ClassRoomSubjectTeachers)
            .HasForeignKey(cst => cst.AcademicYearId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}