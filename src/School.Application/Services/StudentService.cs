using Microsoft.EntityFrameworkCore;
using School.Application.Common;
using School.Application.Common.Cache;
using School.Application.DTOs.Students;
using School.Application.Exceptions;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Domain.Entities;

namespace School.Application.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public StudentService(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
    {
        var enrollmentNumberExists = await _unitOfWork.Students
            .EnrollmentNumberExistsAsync(dto.EnrollmentNumber);

        if (enrollmentNumberExists)
        {
            throw new BadRequestException("A student with the same enrollment number already exists.");
        }

        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            EnrollmentNumber = dto.EnrollmentNumber,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Address = dto.Address,
            AdmissionDate = dto.AdmissionDate,
            NationalId = dto.NationalId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Students.AddAsync(student);
        await _unitOfWork.SaveChangesAsync();
        await _cacheService.RemoveByPrefixAsync(CacheKeys.StudentsListPrefix);

        return MapToDto(student);
    }

    public async Task<PagedResult<StudentDto>> GetAllAsync(StudentQueryParameters queryParameters)
    {
        var cacheKey = CacheKeys.StudentsList(
            queryParameters.Page,
            queryParameters.PageSize,
            queryParameters.Name,
            queryParameters.IsActive,
            queryParameters.Gender?.ToString(),
            queryParameters.SortBy,
            queryParameters.Desc);

        var cachedResult = await _cacheService.GetAsync<PagedResult<StudentDto>>(cacheKey);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        var query = _unitOfWork.Students.GetQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Name))
        {
            var name = queryParameters.Name.ToLower();

            query = query.Where(s =>
                s.FirstName.ToLower().Contains(name) ||
                s.LastName.ToLower().Contains(name));
        }

        if (queryParameters.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == queryParameters.IsActive.Value);
        }

        if (queryParameters.Gender.HasValue)
        {
            query = query.Where(s => s.Gender == queryParameters.Gender.Value);
        }

        var totalCount = await query.CountAsync();

        query = queryParameters.SortBy?.ToLower() switch
        {
            "firstname" => queryParameters.Desc
                ? query.OrderByDescending(s => s.FirstName)
                : query.OrderBy(s => s.FirstName),

            "lastname" => queryParameters.Desc
                ? query.OrderByDescending(s => s.LastName)
                : query.OrderBy(s => s.LastName),

            "createdat" => queryParameters.Desc
                ? query.OrderByDescending(s => s.CreatedAt)
                : query.OrderBy(s => s.CreatedAt),

            _ => query.OrderBy(s => s.CreatedAt)
        };

        var students = await query
            .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToListAsync();

        var result = new PagedResult<StudentDto>
        {
            Items = students.Select(MapToDto),
            Page = queryParameters.Page,
            PageSize = queryParameters.PageSize,
            TotalCount = totalCount
        };

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(2));

        return result;
    }
    public async Task<StudentDto> GetByIdAsync(Guid id)
    {
        var cacheKey = CacheKeys.StudentById(id);

        var cachedStudent = await _cacheService.GetAsync<StudentDto>(cacheKey);
        if (cachedStudent is not null)
        {
            return cachedStudent;
        }

        var student = await _unitOfWork.Students.GetByIdAsync(id);

        if (student is null)
        {
            throw new NotFoundException("Student not found.");
        }

        var studentDto = MapToDto(student);

        await _cacheService.SetAsync(cacheKey, studentDto, TimeSpan.FromMinutes(5));

        return studentDto;
    }

    public async Task<StudentDto> UpdateAsync(Guid id, UpdateStudentDto dto)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);

        if (student is null)
        {
            throw new NotFoundException("Student not found.");
        }

        var enrollmentNumberExists = await _unitOfWork.Students
            .EnrollmentNumberExistsAsync(dto.EnrollmentNumber, id);

        if (enrollmentNumberExists)
        {
            throw new BadRequestException("A student with the same enrollment number already exists.");
        }

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.EnrollmentNumber = dto.EnrollmentNumber;
        student.Email = dto.Email;
        student.PhoneNumber = dto.PhoneNumber;
        student.DateOfBirth = dto.DateOfBirth;
        student.Gender = dto.Gender;
        student.Address = dto.Address;
        student.AdmissionDate = dto.AdmissionDate;
        student.NationalId = dto.NationalId;
        student.IsActive = dto.IsActive;
        student.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Students.Update(student);
        await _unitOfWork.SaveChangesAsync();
        await _cacheService.RemoveAsync(CacheKeys.StudentById(id));
        await _cacheService.RemoveByPrefixAsync(CacheKeys.StudentsListPrefix);

        return MapToDto(student);
    }

    public async Task DeactivateAsync(Guid id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);

        if (student is null)
        {
            throw new NotFoundException("Student not found.");
        }

        if (!student.IsActive)
        {
            throw new BadRequestException("Student is already inactive.");
        }

        student.IsActive = false;
        student.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Students.Update(student);

        await _unitOfWork.SaveChangesAsync();
        await _cacheService.RemoveAsync(CacheKeys.StudentById(id));
        await _cacheService.RemoveByPrefixAsync(CacheKeys.StudentsListPrefix);
    }

    private static StudentDto MapToDto(Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            EnrollmentNumber = student.EnrollmentNumber,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber,
            DateOfBirth = student.DateOfBirth,
            Gender = student.Gender,
            Address = student.Address,
            AdmissionDate = student.AdmissionDate,
            NationalId = student.NationalId,
            IsActive = student.IsActive
        };
    }
}

