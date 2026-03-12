using School.Domain.Common;
using School.Domain.Enums;

namespace School.Domain.Entities;

public class Teacher : BaseEntity
{
    public Guid? UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateOnly HireDate { get; set; }
    public string? Specialization { get; set; }
    public string? NationalId { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }

    public User? User { get; set; }

    public ICollection<ClassRoom> HomeroomClassRooms { get; set; } = new List<ClassRoom>();
    public ICollection<ClassRoomSubjectTeacher> ClassRoomSubjectTeachers { get; set; } = new List<ClassRoomSubjectTeacher>();
}