using CollegeManagementSystem.Data.Entities;

namespace CollegeManagementSystem.Repositories.Interfaces;

public interface IEnrollmentRepository
{
    Task<List<Enrollment>> GetAllAsync();
    Task<List<Enrollment>> GetFullDetailsAsync();
    Task<List<Enrollment>> GetByDateAsync(DateTime date);
    Task<Enrollment?> GetByIdAsync(string studentId, int courseId);
    Task<bool> EnrollmentExistsAsync(string studentId, int courseId);
    Task<bool> StudentExistsAsync(string studentId);
    Task<bool> CourseExistsAsync(int courseId);
    Task<int> CountAsync();
    Task AddAsync(Enrollment enrollment);
    Task AddRangeAsync(IEnumerable<Enrollment> enrollments);
    void Remove(Enrollment enrollment);
    Task<int> SaveChangesAsync();
}
