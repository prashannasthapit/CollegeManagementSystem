using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Repositories.Implementations;

public class StudentRepository(AppDbContext context) : IStudentRepository
{
    public Task<List<Student>> GetAllAsync() => context.Students.AsNoTracking().ToListAsync();

    public Task<Student?> GetByIdAsync(string id) => context.Students.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<List<Course>> GetCoursesAsync(string studentId)
    {
        return await context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Select(e => e.Course)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<List<Student>> GetWithCoursesAsync()
    {
        return context.Students
            .AsNoTracking()
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .ToListAsync();
    }

    public Task<List<Student>> GetFullDetailsAsync()
    {
        return context.Students
            .AsNoTracking()
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .ThenInclude(c => c.Modules)
            .ToListAsync();
    }

    public Task<bool> ExistsAsync(string id) => context.Students.AnyAsync(s => s.Id == id);

    public Task<int> CountAsync() => context.Students.CountAsync();

    public Task AddAsync(Student student) => context.Students.AddAsync(student).AsTask();

    public Task AddRangeAsync(IEnumerable<Student> students) => context.Students.AddRangeAsync(students);

    public void Remove(Student student) => context.Students.Remove(student);

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}
