using Microsoft.EntityFrameworkCore;
using School.Application.Interfaces.Repositories;
using School.Domain.Entities;
using School.Infrastructure.Persistence.Context;

namespace School.Infrastructure.Persistence.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(SchoolDbContext context) : base(context)
    {
    }

    public async Task<bool> EnrollmentNumberExistsAsync(string enrollmentNumber)
    {
        return await _dbSet.AnyAsync(s => s.EnrollmentNumber == enrollmentNumber);
    }
}