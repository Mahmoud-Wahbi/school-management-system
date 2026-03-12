using School.Domain.Common;

namespace School.Domain.Entities;

public class ClassRoomSubjectTeacher : BaseEntity
{
    public Guid ClassRoomId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }
    public Guid AcademicYearId { get; set; }

    public bool IsActive { get; set; }

    public ClassRoom ClassRoom { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
    public Teacher Teacher { get; set; } = null!;
    public AcademicYear AcademicYear { get; set; } = null!;
}