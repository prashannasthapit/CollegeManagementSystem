using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Repositories.Implementations;

public class CourseRepository(AppDbContext context) : ICourseRepository
{
    public Task<List<Course>> GetAllAsync()
    {
        return context.Courses
            .AsNoTracking()
            .Include(c => c.Modules)
            .ToListAsync();
    }

    public Task<Course?> GetByIdWithModulesAsync(int id)
    {
        return context.Courses
            .AsNoTracking()
            .Include(c => c.Modules)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<Course?> GetByIdWithStudentsAsync(int id)
    {
        return context.Courses
            .AsNoTracking()
            .Include(c => c.Enrollments)
            .ThenInclude(e => e.Student)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<Course?> GetByIdWithDetailsAsync(int id)
    {
        return context.Courses
            .AsNoTracking()
            .Include(c => c.Modules)
            .ThenInclude(m => m.ModuleInstructors)
            .ThenInclude(mi => mi.Instructor)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<List<Course>> GetWithDetailsAsync()
    {
        return context.Courses
            .AsNoTracking()
            .Include(c => c.Modules)
            .ThenInclude(m => m.ModuleInstructors)
            .ThenInclude(mi => mi.Instructor)
            .ToListAsync();
    }

    public async Task<List<Module>> GetModulesByCourseIdAsync(int courseId)
    {
        return await context.Modules
            .Where(m => m.CourseId == courseId)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<bool> ExistsAsync(int id) => context.Courses.AnyAsync(c => c.Id == id);

    public Task<int> CountAsync() => context.Courses.CountAsync();

    public async Task<int> SumModuleCreditsAsync()
    {
        return await context.Modules.SumAsync(m => (int?)m.Credits) ?? 0;
    }

    public async Task<List<(Course course, int enrollmentCount)>> GetTopEnrolledAsync(int take)
    {
        return await context.Courses
            .AsNoTracking()
            .Select(c => new { Course = c, EnrollmentCount = c.Enrollments.Count })
            .OrderByDescending(x => x.EnrollmentCount)
            .ThenBy(x => x.Course.Name)
            .Take(take)
            .Select(x => ValueTuple.Create(x.Course, x.EnrollmentCount))
            .ToListAsync();
    }

    public Task AddAsync(Course course) => context.Courses.AddAsync(course).AsTask();

    public Task AddRangeAsync(IEnumerable<Course> courses) => context.Courses.AddRangeAsync(courses);

    public void Remove(Course course) => context.Courses.Remove(course);

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();
}
