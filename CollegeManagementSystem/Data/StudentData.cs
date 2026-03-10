using CollegeManagementSystem.Dtos;

namespace CollegeManagementSystem.Data;

public class StudentData
{
    public static List<StudentDto> Students { get; } =
    [
        new() { Id = "1", Name = "Alice Johnson", Age = 20, Course = "Computer Science" },
        new() { Id = "2", Name = "Bob Smith", Age = 22, Course = "Mathematics" },
        new() { Id = "3", Name = "Charlie Brown", Age = 19, Course = "Physics" }
    ];
}