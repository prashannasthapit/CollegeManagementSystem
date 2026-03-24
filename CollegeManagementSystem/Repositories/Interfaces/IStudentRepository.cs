using CollegeManagementSystem.Data.Entities;

namespace CollegeManagementSystem.Repositories.Interfaces;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(string id);
    Task<List<Course>> GetCoursesAsync(string studentId);
    Task<List<Student>> GetWithCoursesAsync();
    Task<List<Student>> GetFullDetailsAsync();
    Task<bool> ExistsAsync(string id);
    Task<int> CountAsync();
    Task AddAsync(Student student);
    Task AddRangeAsync(IEnumerable<Student> students);
    void Remove(Student student);
    Task<int> SaveChangesAsync();
}
