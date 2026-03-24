using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Dtos;

public class CourseCreateRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Range(1, 20)]
    public int DurationYears { get; set; }

    public List<CourseModuleCreateDto> Modules { get; set; } = [];
}

public class CourseUpdateRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Range(1, 20)]
    public int DurationYears { get; set; }

    public List<CourseModuleCreateDto> Modules { get; set; } = [];
}

public class CourseModuleCreateDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    [Range(1, 60)]
    public int Credits { get; set; }
}

public class CourseResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DurationYears { get; set; }
    public int ModuleCount { get; set; }
}

public class CourseDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DurationYears { get; set; }
    public List<CourseModuleDto> Modules { get; set; } = [];
}

public class CourseModuleDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Credits { get; set; }
}

public class CourseStudentDto
{
    public string StudentId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime EnrolledDate { get; set; }
}

public class CourseWithDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int DurationYears { get; set; }
    public List<CourseModuleWithInstructorsDto> Modules { get; set; } = [];
}

public class CourseModuleWithInstructorsDto
{
    public int ModuleId { get; set; }
    public string Title { get; set; } = null!;
    public int Credits { get; set; }
    public List<InstructorBasicDto> Instructors { get; set; } = [];
}

public class InstructorBasicDto
{
    public int InstructorId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class TopEnrolledCourseDto
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public int EnrollmentCount { get; set; }
}
