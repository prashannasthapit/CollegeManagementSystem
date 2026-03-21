using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Data.Entities;

public class Course
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Range(1, 20)]
    public int DurationYears { get; set; }

    public ICollection<Module> Modules { get; set; } = new List<Module>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}