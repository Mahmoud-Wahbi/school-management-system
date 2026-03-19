using School.Domain.Common;

namespace School.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public ICollection<Student> OwnedStudents { get; set; } = new List<Student>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public Teacher? Teacher { get; set; }
}