using Microsoft.AspNetCore.Mvc;
using School.API.Common;
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
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllAsync();

        var response = ApiResponse<IEnumerable<StudentDto>>.SuccessResponse(
            students,
            "Students retrieved successfully.");

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var student = await _studentService.GetByIdAsync(id);

        if (student is null)
        {
            var notFoundResponse = ApiResponse<StudentDto>.FailureResponse("Student not found.");
            return NotFound(notFoundResponse);
        }

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
}