using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Data.Entities;

public class Instructor
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public ICollection<ModuleInstructor> ModuleInstructors { get; set; } = new List<ModuleInstructor>();
}
