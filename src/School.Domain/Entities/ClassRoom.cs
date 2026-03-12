using School.Domain.Common;

namespace School.Domain.Entities;

public class ClassRoom : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int GradeLevel { get; set; }
    public string Section { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public Guid? HomeroomTeacherId { get; set; }
    public Guid AcademicYearId { get; set; }

    public bool IsActive { get; set; }

    public Teacher? HomeroomTeacher { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<ClassRoomSubjectTeacher> ClassRoomSubjectTeachers { get; set; } = new List<ClassRoomSubjectTeacher>();
}