using CollegeManagementSystem.Data.Entities;

namespace CollegeManagementSystem.Repositories.Interfaces;

public interface ICourseRepository
{
    Task<List<Course>> GetAllAsync();
    Task<Course?> GetByIdWithModulesAsync(int id);
    Task<Course?> GetByIdWithStudentsAsync(int id);
    Task<Course?> GetByIdWithDetailsAsync(int id);
    Task<List<Course>> GetWithDetailsAsync();
    Task<List<Module>> GetModulesByCourseIdAsync(int courseId);
    Task<bool> ExistsAsync(int id);
    Task<int> CountAsync();
    Task<int> SumModuleCreditsAsync();
    Task<List<(Course course, int enrollmentCount)>> GetTopEnrolledAsync(int take);
    Task AddAsync(Course course);
    Task AddRangeAsync(IEnumerable<Course> courses);
    void Remove(Course course);
    Task<int> SaveChangesAsync();
}
