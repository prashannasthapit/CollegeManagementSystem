using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Dtos;

public class EnrollmentCreateRequestDto
{
    [Required]
    public string StudentId { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }

    public DateTime? EnrolledDate { get; set; }
}

public class EnrollmentResponseDto
{
    public string StudentId { get; set; } = null!;
    public int CourseId { get; set; }
    public DateTime EnrolledDate { get; set; }
}

public class EnrollmentFullDetailsDto
{
    public string StudentId { get; set; } = null!;
    public string StudentName { get; set; } = null!;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = null!;
    public DateTime EnrolledDate { get; set; }
}
