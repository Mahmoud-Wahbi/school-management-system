using School.Application.DTOs.Students;
using School.Application.Interfaces.Repositories;
using School.Application.Interfaces.Services;
using School.Domain.Entities;
using School.Application.Exceptions;

namespace School.Application.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;

    public StudentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

        return MapToDto(student);
    }

    public async Task<IEnumerable<StudentDto>> GetAllAsync()
    {
        var students = await _unitOfWork.Students.GetAllAsync();

        return students.Select(MapToDto);
    }

    public async Task<StudentDto> GetByIdAsync(Guid id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);

        if (student is null)
        {
            throw new NotFoundException("Student not found.");
        }

        return MapToDto(student);
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
    }
}