using School.Domain.Entities;

namespace School.Application.Interfaces.Common;

public interface IStudentAuthorizationService
{
    Task EnsureCanAccessAsync(Student student);
}