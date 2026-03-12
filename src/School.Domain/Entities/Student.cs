using School.Domain.Common;
using School.Domain.Enums;

namespace School.Domain.Entities;

public class Student : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EnrollmentNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    public DateOnly AdmissionDate { get; set; }
    public string? NationalId { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}