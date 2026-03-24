using System.ComponentModel.DataAnnotations;

namespace CollegeManagementSystem.Dtos;

public class ModuleInstructorCreateRequestDto
{
    [Range(1, int.MaxValue)]
    public int ModuleId { get; set; }

    [Range(1, int.MaxValue)]
    public int InstructorId { get; set; }
}

public class ModuleInstructorDetailsDto
{
    public int ModuleId { get; set; }
    public string ModuleTitle { get; set; } = null!;
    public int InstructorId { get; set; }
    public string InstructorName { get; set; } = null!;
}

public class InstructorAssignmentCountDto
{
    public int InstructorId { get; set; }
    public string InstructorName { get; set; } = null!;
    public int AssignmentCount { get; set; }
}
