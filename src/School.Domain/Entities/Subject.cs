using School.Domain.Common;

namespace School.Domain.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public ICollection<ClassRoomSubjectTeacher> ClassRoomSubjectTeachers { get; set; } = new List<ClassRoomSubjectTeacher>();
}