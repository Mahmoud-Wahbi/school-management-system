using School.Domain.Enums;

namespace School.Application.DTOs.Students;

public class StudentQueryParameters
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public Gender? Gender { get; set; }
}