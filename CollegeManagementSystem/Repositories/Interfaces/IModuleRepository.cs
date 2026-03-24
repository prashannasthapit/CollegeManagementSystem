using CollegeManagementSystem.Data.Entities;

namespace CollegeManagementSystem.Repositories.Interfaces;

public interface IModuleRepository
{
    Task<List<Module>> GetAllAsync();
    Task<Module?> GetByIdAsync(int id);
    Task<Module?> GetByIdWithCourseAsync(int id);
    Task<Module?> GetByIdWithInstructorsAsync(int id);
    Task<List<Module>> GetWithCourseAsync();
    Task<List<Module>> GetHighCreditAsync(int minCredits);
    Task<List<Module>> GetByIdsAsync(IEnumerable<int> ids);
    Task<int> CountAsync();
    Task<bool> ExistsAsync(int id);
    Task AddAsync(Module module);
    Task AddRangeAsync(IEnumerable<Module> modules);
    void Remove(Module module);
    Task<int> SaveChangesAsync();
}
