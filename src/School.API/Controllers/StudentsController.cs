using Microsoft.AspNetCore.Mvc;
using School.API.Common;
using School.Application.Common;
using School.Application.DTOs.Students;
using School.Application.Interfaces.Services;

namespace School.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var students = await _studentService.GetAllAsync(page, pageSize);

        var response = ApiResponse<PagedResult<StudentDto>>.SuccessResponse(
            students,
            "Students retrieved successfully");

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var student = await _studentService.GetByIdAsync(id);

        var response = ApiResponse<StudentDto>.SuccessResponse(
            student,
            "Student retrieved successfully.");

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentDto dto)
    {
        var createdStudent = await _studentService.CreateAsync(dto);

        var response = ApiResponse<StudentDto>.SuccessResponse(
            createdStudent,
            "Student created successfully.");

        return CreatedAtAction(nameof(GetById), new { id = createdStudent.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateStudentDto dto)
    {
        var updatedStudent = await _studentService.UpdateAsync(id, dto);

        var response = ApiResponse<StudentDto>.SuccessResponse(
            updatedStudent,
            "Student updated successfully.");

        return Ok(response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _studentService.DeactivateAsync(id);

        var response = ApiResponse<string>.SuccessResponse(
            "Student deactivated successfully.");

        return Ok(response);
    }

}