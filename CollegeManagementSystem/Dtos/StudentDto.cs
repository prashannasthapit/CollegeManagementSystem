using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Dtos;

public class StudentDto
{
    [Required] public string Id { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = null!;

    [Range(1, 100, ErrorMessage = "Age must be between 1 and 100.")]
    public int Age { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters.")]
    public string Course { get; set; } = null!;
}