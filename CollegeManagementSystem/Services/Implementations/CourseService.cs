using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Repositories.Interfaces;
using CollegeManagementSystem.Services.Common;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Services.Implementations;

public class CourseService(ICourseRepository courseRepository, AppDbContext dbContext) : ICourseService
{
    public async Task<List<CourseResponseDto>> GetAllAsync()
    {
        var courses = await courseRepository.GetAllAsync();
        return courses.Select(c => new CourseResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            DurationYears = c.DurationYears,
            ModuleCount = c.Modules.Count
        }).ToList();
    }

    public async Task<CourseDetailsDto?> GetByIdAsync(int id)
    {
        var course = await courseRepository.GetByIdWithModulesAsync(id);
        return course is null ? null : MapCourseDetails(course);
    }

    public async Task<List<CourseModuleDto>?> GetModulesAsync(int id)
    {
        if (!await courseRepository.ExistsAsync(id))
        {
            return null;
        }

        var modules = await courseRepository.GetModulesByCourseIdAsync(id);
        return modules.Select(m => new CourseModuleDto
        {
            Id = m.Id,
            Title = m.Title,
            Credits = m.Credits
        }).ToList();
    }

    public async Task<List<CourseStudentDto>?> GetStudentsAsync(int id)
    {
        var course = await courseRepository.GetByIdWithStudentsAsync(id);
        if (course is null)
        {
            return null;
        }

        return course.Enrollments.Select(e => new CourseStudentDto
        {
            StudentId = e.StudentId,
            FullName = $"{e.Student.FirstName} {e.Student.LastName}".Trim(),
            Email = e.Student.Email,
            EnrolledDate = e.EnrolledDate
        }).ToList();
    }

    public async Task<ServiceResult<CourseDetailsDto>> CreateAsync(CourseCreateRequestDto dto)
    {
        var course = new Course
        {
            Name = dto.Name.Trim(),
            DurationYears = dto.DurationYears,
            Modules = dto.Modules.Select(m => new Module
            {
                Title = m.Title.Trim(),
                Credits = m.Credits
            }).ToList()
        };

        await courseRepository.AddAsync(course);
        await courseRepository.SaveChangesAsync();

        return ServiceResult<CourseDetailsDto>.Ok(MapCourseDetails(course));
    }

    public async Task<ServiceResult<ModuleResponseDto>> AddModuleAsync(int courseId, CourseModuleCreateDto dto)
    {
        var courseExists = await courseRepository.ExistsAsync(courseId);
        if (!courseExists)
        {
            return ServiceResult<ModuleResponseDto>.Fail(ServiceErrorType.NotFound, "Course not found.");
        }

        var module = new Module
        {
            Title = dto.Title.Trim(),
            Credits = dto.Credits,
            CourseId = courseId
        };

        await dbContext.Modules.AddAsync(module);
        await dbContext.SaveChangesAsync();

        return ServiceResult<ModuleResponseDto>.Ok(new ModuleResponseDto
        {
            Id = module.Id,
            Title = module.Title,
            Credits = module.Credits,
            CourseId = module.CourseId
        });
    }

    public async Task<ServiceResult> UpdateAsync(int id, CourseUpdateRequestDto dto)
    {
        var course = await dbContext.Courses
            .Include(c => c.Modules)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Course not found.");
        }

        course.Name = dto.Name.Trim();
        course.DurationYears = dto.DurationYears;

        dbContext.Modules.RemoveRange(course.Modules);
        var newModules = dto.Modules.Select(m => new Module
        {
            Title = m.Title.Trim(),
            Credits = m.Credits,
            CourseId = id
        }).ToList();

        await dbContext.Modules.AddRangeAsync(newModules);
        await dbContext.SaveChangesAsync();

        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var course = await dbContext.Courses.FirstOrDefaultAsync(c => c.Id == id);
        if (course is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Course not found.");
        }

        courseRepository.Remove(course);
        await courseRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<int>> BulkCreateAsync(List<CourseCreateRequestDto> dtos)
    {
        if (dtos.Count == 0)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Request body cannot be empty.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var courses = dtos.Select(d => new Course
            {
                Name = d.Name.Trim(),
                DurationYears = d.DurationYears,
                Modules = d.Modules.Select(m => new Module
                {
                    Title = m.Title.Trim(),
                    Credits = m.Credits
                }).ToList()
            }).ToList();

            await courseRepository.AddRangeAsync(courses);
            var inserted = await courseRepository.SaveChangesAsync();
            await transaction.CommitAsync();
            return ServiceResult<int>.Ok(inserted);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<CourseWithDetailsDto>> GetWithDetailsAsync()
    {
        var courses = await courseRepository.GetWithDetailsAsync();
        return courses.Select(c => new CourseWithDetailsDto
        {
            Id = c.Id,
            Name = c.Name,
            DurationYears = c.DurationYears,
            Modules = c.Modules.Select(m => new CourseModuleWithInstructorsDto
            {
                ModuleId = m.Id,
                Title = m.Title,
                Credits = m.Credits,
                Instructors = m.ModuleInstructors.Select(mi => new InstructorBasicDto
                {
                    InstructorId = mi.InstructorId,
                    FullName = $"{mi.Instructor.FirstName} {mi.Instructor.LastName}".Trim(),
                    Email = mi.Instructor.Email
                }).ToList()
            }).ToList()
        }).ToList();
    }

    public Task<int> CountAsync() => courseRepository.CountAsync();

    public Task<int> TotalCreditsAsync() => courseRepository.SumModuleCreditsAsync();

    public async Task<List<TopEnrolledCourseDto>> TopEnrolledAsync(int take = 5)
    {
        var courses = await courseRepository.GetTopEnrolledAsync(take);
        return courses.Select(c => new TopEnrolledCourseDto
        {
            CourseId = c.course.Id,
            CourseName = c.course.Name,
            EnrollmentCount = c.enrollmentCount
        }).ToList();
    }

    private static CourseDetailsDto MapCourseDetails(Course course)
    {
        return new CourseDetailsDto
        {
            Id = course.Id,
            Name = course.Name,
            DurationYears = course.DurationYears,
            Modules = course.Modules.Select(m => new CourseModuleDto
            {
                Id = m.Id,
                Title = m.Title,
                Credits = m.Credits
            }).ToList()
        };
    }
}
