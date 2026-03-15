using School.Application.DTOs.Students;

namespace School.Application.Interfaces.Services;

public interface IStudentService
{
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
    Task<IEnumerable<StudentDto>> GetAllAsync();
    Task<StudentDto> GetByIdAsync(Guid id);
}