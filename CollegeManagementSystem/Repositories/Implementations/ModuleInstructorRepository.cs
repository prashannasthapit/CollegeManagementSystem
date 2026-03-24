using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Repositories.Implementations;

public class ModuleInstructorRepository(AppDbContext context) : IModuleInstructorRepository
{
    public Task<ModuleInstructor?> GetByIdAsync(int moduleId, int instructorId)
    {
        return context.ModuleInstructors
            .FirstOrDefaultAsync(mi => mi.ModuleId == moduleId && mi.InstructorId == instructorId);
    }

    public Task<bool> AssignmentExistsAsync(int moduleId, int instructorId)
    {
        return context.ModuleInstructors.AnyAsync(mi => mi.ModuleId == moduleId && mi.InstructorId == instructorId);
    }

    public Task<bool> ModuleExistsAsync(int moduleId) => context.Modules.AnyAsync(m => m.Id == moduleId);

    public Task<bool> InstructorExistsAsync(int instructorId) => context.Instructors.AnyAsync(i => i.Id == instructorId);

    public Task<List<ModuleInstructor>> GetFullDetailsAsync()
    {
        return context.ModuleInstructors
            .AsNoTracking()
            .Include(mi => mi.Module)
            .Include(mi => mi.Instructor)
            .ToListAsync();
    }

    public Task<int> CountAsync() => context.ModuleInstructors.CountAsync();

    public async Task<List<(int instructorId, string firstName, string lastName, int assignmentCount)>> GetInstructorAssignmentCountsAsync()
    {
        return await context.ModuleInstructors
            .AsNoTracking()
            .Include(mi => mi.Instructor)
            .GroupBy(mi => new { mi.InstructorId, mi.Instructor.FirstName, mi.Instructor.LastName })
            .Select(g => ValueTuple.Create(g.Key.InstructorId, g.Key.FirstName, g.Key.LastName, g.Count()))
            .OrderByDescending(x => x.Item4)
            .ThenBy(x => x.Item3)
            .ToListAsync();
    }

    public Task AddAsync(ModuleInstructor assignment) => context.ModuleInstructors.AddAsync(assignment).AsTask();

    public Task AddRangeAsync(IEnumerable<ModuleInstructor> assignments) => context.ModuleInstructors.AddRangeAsync(assignments);

    public void Remove(ModuleInstructor assignment) => context.ModuleInstructors.Remove(assignment);

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}
