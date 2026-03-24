using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Dtos;

public class InstructorCreateRequestDto
{
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

    [Required]
    public DateTime HireDate { get; set; }
}

public class InstructorUpdateRequestDto
{
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

    [Required]
    public DateTime HireDate { get; set; }
}

public class InstructorResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }
}

public class InstructorWithModulesDto
{
    public int InstructorId { get; set; }
    public string FullName { get; set; } = null!;
    public List<InstructorModuleDto> Modules { get; set; } = [];
}

public class InstructorModuleDto
{
    public int ModuleId { get; set; }
    public string Title { get; set; } = null!;
    public int Credits { get; set; }
}

public class InstructorModuleCountDto
{
    public int InstructorId { get; set; }
    public string FullName { get; set; } = null!;
    public int ModuleCount { get; set; }
}
