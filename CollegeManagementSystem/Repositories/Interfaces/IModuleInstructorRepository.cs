using CollegeManagementSystem.Data.Entities;

namespace CollegeManagementSystem.Repositories.Interfaces;

public interface IModuleInstructorRepository
{
    Task<ModuleInstructor?> GetByIdAsync(int moduleId, int instructorId);
    Task<bool> AssignmentExistsAsync(int moduleId, int instructorId);
    Task<bool> ModuleExistsAsync(int moduleId);
    Task<bool> InstructorExistsAsync(int instructorId);
    Task<List<ModuleInstructor>> GetFullDetailsAsync();
    Task<int> CountAsync();
    Task<List<(int instructorId, string firstName, string lastName, int assignmentCount)>> GetInstructorAssignmentCountsAsync();
    Task AddAsync(ModuleInstructor assignment);
    Task AddRangeAsync(IEnumerable<ModuleInstructor> assignments);
    void Remove(ModuleInstructor assignment);
    Task<int> SaveChangesAsync();
}
