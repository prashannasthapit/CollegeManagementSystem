using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Data.Entities;

public class Module
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    [Range(1, 60)]
    public int Credits { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public ICollection<ModuleInstructor> ModuleInstructors { get; set; } = new List<ModuleInstructor>();
}
