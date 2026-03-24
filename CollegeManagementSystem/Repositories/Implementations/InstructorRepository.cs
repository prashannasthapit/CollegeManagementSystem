using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Repositories.Implementations;

public class InstructorRepository(AppDbContext context) : IInstructorRepository
{
    public Task<List<Instructor>> GetAllAsync() => context.Instructors.AsNoTracking().ToListAsync();

    public Task<Instructor?> GetByIdAsync(int id) => context.Instructors.FirstOrDefaultAsync(i => i.Id == id);

    public Task<List<Instructor>> GetWithModulesAsync()
    {
        return context.Instructors
            .AsNoTracking()
            .Include(i => i.ModuleInstructors)
            .ThenInclude(mi => mi.Module)
            .ToListAsync();
    }

    public Task<int> CountAsync() => context.Instructors.CountAsync();

    public Task<bool> ExistsAsync(int id) => context.Instructors.AnyAsync(i => i.Id == id);

    public Task<List<int>> GetDistinctHireYearsAsync()
    {
        return context.Instructors
            .AsNoTracking()
            .Select(i => i.HireDate.Year)
            .Distinct()
            .OrderBy(y => y)
            .ToListAsync();
    }

    public async Task<List<(Instructor instructor, int moduleCount)>> GetModuleCountsAsync()
    {
        return await context.Instructors
            .AsNoTracking()
            .Select(i => new { Instructor = i, ModuleCount = i.ModuleInstructors.Count })
            .OrderByDescending(x => x.ModuleCount)
            .ThenBy(x => x.Instructor.LastName)
            .Select(x => ValueTuple.Create(x.Instructor, x.ModuleCount))
            .ToListAsync();
    }

    public Task AddAsync(Instructor instructor) => context.Instructors.AddAsync(instructor).AsTask();

    public Task AddRangeAsync(IEnumerable<Instructor> instructors) => context.Instructors.AddRangeAsync(instructors);

    public void Remove(Instructor instructor) => context.Instructors.Remove(instructor);

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}
