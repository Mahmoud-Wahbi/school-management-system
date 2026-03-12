using School.Domain.Common;

namespace School.Domain.Entities;

public class AcademicYear : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsCurrent { get; set; }
    public bool IsActive { get; set; }

    public ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<ClassRoomSubjectTeacher> ClassRoomSubjectTeachers { get; set; } = new List<ClassRoomSubjectTeacher>();
}