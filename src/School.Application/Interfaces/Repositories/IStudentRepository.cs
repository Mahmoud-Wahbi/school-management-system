using School.Domain.Entities;

namespace School.Application.Interfaces.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<bool> EnrollmentNumberExistsAsync(string enrollmentNumber);
}