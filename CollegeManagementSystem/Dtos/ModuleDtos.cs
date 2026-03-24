using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Dtos;

public class ModuleCreateRequestDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    [Range(1, 60)]
    public int Credits { get; set; }

    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }
}

public class ModuleUpdateRequestDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = null!;

    [Range(1, 60)]
    public int Credits { get; set; }

    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }
}

public class ModuleResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Credits { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
}

public class ModuleInstructorDto
{
    public int InstructorId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class BulkUpdateModuleCreditsRequestDto
{
    [Required]
    public List<ModuleCreditUpdateItemDto> Updates { get; set; } = [];
}

public class ModuleCreditUpdateItemDto
{
    [Range(1, int.MaxValue)]
    public int ModuleId { get; set; }

    [Range(1, 60)]
    public int Credits { get; set; }
}
