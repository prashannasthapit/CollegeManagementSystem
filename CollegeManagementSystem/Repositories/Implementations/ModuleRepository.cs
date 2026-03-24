using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Repositories.Implementations;

public class ModuleRepository(AppDbContext context) : IModuleRepository
{
    public Task<List<Module>> GetAllAsync() => context.Modules.AsNoTracking().ToListAsync();

    public Task<Module?> GetByIdAsync(int id) => context.Modules.FirstOrDefaultAsync(m => m.Id == id);

    public Task<Module?> GetByIdWithCourseAsync(int id)
    {
        return context.Modules
            .AsNoTracking()
            .Include(m => m.Course)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public Task<Module?> GetByIdWithInstructorsAsync(int id)
    {
        return context.Modules
            .AsNoTracking()
            .Include(m => m.ModuleInstructors)
            .ThenInclude(mi => mi.Instructor)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public Task<List<Module>> GetWithCourseAsync()
    {
        return context.Modules
            .AsNoTracking()
            .Include(m => m.Course)
            .ToListAsync();
    }

    public Task<List<Module>> GetHighCreditAsync(int minCredits)
    {
        return context.Modules
            .AsNoTracking()
            .Where(m => m.Credits > minCredits)
            .ToListAsync();
    }

    public Task<List<Module>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return context.Modules
            .Where(m => ids.Contains(m.Id))
            .ToListAsync();
    }

    public Task<int> CountAsync() => context.Modules.CountAsync();

    public Task<bool> ExistsAsync(int id) => context.Modules.AnyAsync(m => m.Id == id);

    public Task AddAsync(Module module) => context.Modules.AddAsync(module).AsTask();

    public Task AddRangeAsync(IEnumerable<Module> modules) => context.Modules.AddRangeAsync(modules);

    public void Remove(Module module) => context.Modules.Remove(module);

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}
