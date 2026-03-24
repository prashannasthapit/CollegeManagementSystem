using CollegeManagementSystem.Data.Entities;

namespace CollegeManagementSystem.Repositories.Interfaces;

public interface IInstructorRepository
{
    Task<List<Instructor>> GetAllAsync();
    Task<Instructor?> GetByIdAsync(int id);
    Task<List<Instructor>> GetWithModulesAsync();
    Task<int> CountAsync();
    Task<List<int>> GetDistinctHireYearsAsync();
    Task<List<(Instructor instructor, int moduleCount)>> GetModuleCountsAsync();
    Task<bool> ExistsAsync(int id);
    Task AddAsync(Instructor instructor);
    Task AddRangeAsync(IEnumerable<Instructor> instructors);
    void Remove(Instructor instructor);
    Task<int> SaveChangesAsync();
}
