namespace CollegeManagementSystem.Data.Entities;

public class ModuleInstructor
{
    public int ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;
}
