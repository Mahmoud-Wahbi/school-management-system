using School.Application.Common;
using School.Application.DTOs.Students;

namespace School.Application.Interfaces.Services;

public interface IStudentService
{
    Task<StudentDto> CreateAsync(CreateStudentDto dto);
    Task<PagedResult<StudentDto>> GetAllAsync(StudentQueryParameters queryParameters);
    Task<StudentDto> GetByIdAsync(Guid id);
    Task<StudentDto> UpdateAsync(Guid id, UpdateStudentDto dto);
    Task DeactivateAsync(Guid id);
}