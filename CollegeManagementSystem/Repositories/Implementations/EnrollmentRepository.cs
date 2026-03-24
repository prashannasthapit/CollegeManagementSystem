using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Repositories.Implementations;

public class EnrollmentRepository(AppDbContext context) : IEnrollmentRepository
{
    public Task<List<Enrollment>> GetAllAsync() => context.Enrollments.AsNoTracking().ToListAsync();

    public Task<List<Enrollment>> GetFullDetailsAsync()
    {
        return context.Enrollments
            .AsNoTracking()
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ToListAsync();
    }

    public Task<List<Enrollment>> GetByDateAsync(DateTime date)
    {
        var targetDate = date.Date;
        return context.Enrollments
            .AsNoTracking()
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Where(e => e.EnrolledDate.Date == targetDate)
            .ToListAsync();
    }

    public Task<Enrollment?> GetByIdAsync(string studentId, int courseId)
    {
        return context.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);
    }

    public Task<bool> EnrollmentExistsAsync(string studentId, int courseId)
    {
        return context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
    }

    public Task<bool> StudentExistsAsync(string studentId) => context.Students.AnyAsync(s => s.Id == studentId);

    public Task<bool> CourseExistsAsync(int courseId) => context.Courses.AnyAsync(c => c.Id == courseId);

    public Task<int> CountAsync() => context.Enrollments.CountAsync();

    public Task AddAsync(Enrollment enrollment) => context.Enrollments.AddAsync(enrollment).AsTask();

    public Task AddRangeAsync(IEnumerable<Enrollment> enrollments) => context.Enrollments.AddRangeAsync(enrollments);

    public void Remove(Enrollment enrollment) => context.Enrollments.Remove(enrollment);

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}
