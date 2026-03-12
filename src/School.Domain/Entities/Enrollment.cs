using School.Domain.Common;
using School.Domain.Enums;

namespace School.Domain.Entities;

public class Enrollment : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid ClassRoomId { get; set; }
    public Guid AcademicYearId { get; set; }

    public DateOnly EnrollmentDate { get; set; }
    public EnrollmentStatus Status { get; set; }
    public string? Notes { get; set; }

    public Student Student { get; set; } = null!;
    public ClassRoom ClassRoom { get; set; } = null!;
    public AcademicYear AcademicYear { get; set; } = null!;
}