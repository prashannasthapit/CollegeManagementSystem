using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Data.Entities;

public class Student
{
    [Key]
    public string Id { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}