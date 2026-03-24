using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Dtos;

public class StudentCreateRequestDto
{
    [StringLength(50)]
    public string? Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = null!;
}

public class StudentUpdateRequestDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = null!;
}

public class StudentResponseDto
{
    public string Id { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = null!;
}

public class StudentCourseDto
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public DateTime EnrolledDate { get; set; }
}

public class StudentWithCoursesDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<StudentCourseDto> Courses { get; set; } = [];
}

public class StudentFullDetailsDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<StudentCourseFullDto> Courses { get; set; } = [];
}

public class StudentCourseFullDto
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public List<StudentModuleDto> Modules { get; set; } = [];
}

public class StudentModuleDto
{
    public int ModuleId { get; set; }
    public string Title { get; set; } = null!;
    public int Credits { get; set; }
}