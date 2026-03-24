using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Repositories.Interfaces;
using CollegeManagementSystem.Services.Common;
using CollegeManagementSystem.Services.Interfaces;

namespace CollegeManagementSystem.Services.Implementations;

public class StudentService(IStudentRepository studentRepository, AppDbContext dbContext) : IStudentService
{
    public async Task<List<StudentResponseDto>> GetAllAsync()
    {
        var students = await studentRepository.GetAllAsync();
        return students.Select(MapStudent).ToList();
    }

    public async Task<StudentResponseDto?> GetByIdAsync(string id)
    {
        var student = await studentRepository.GetByIdAsync(id);
        return student is null ? null : MapStudent(student);
    }

    public async Task<List<StudentCourseDto>?> GetCoursesAsync(string id)
    {
        var student = await studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return null;
        }

        var withCourses = await studentRepository.GetWithCoursesAsync();
        var target = withCourses.First(s => s.Id == id);

        return target.Enrollments.Select(e => new StudentCourseDto
        {
            CourseId = e.CourseId,
            CourseName = e.Course.Name,
            EnrolledDate = e.EnrolledDate
        }).ToList();
    }

    public async Task<ServiceResult<StudentResponseDto>> CreateAsync(StudentCreateRequestDto dto)
    {
        var studentId = string.IsNullOrWhiteSpace(dto.Id) ? Guid.NewGuid().ToString("N") : dto.Id.Trim();

        if (await studentRepository.ExistsAsync(studentId))
        {
            return ServiceResult<StudentResponseDto>.Fail(ServiceErrorType.Conflict, "Student with the same id already exists.");
        }

        var student = new Student
        {
            Id = studentId,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            DateOfBirth = dto.DateOfBirth,
            Phone = dto.Phone?.Trim() ?? string.Empty,
            Email = dto.Email.Trim()
        };

        await studentRepository.AddAsync(student);
        await studentRepository.SaveChangesAsync();

        return ServiceResult<StudentResponseDto>.Ok(MapStudent(student));
    }

    public async Task<ServiceResult> UpdateAsync(string id, StudentUpdateRequestDto dto)
    {
        var student = await studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Student not found.");
        }

        student.FirstName = dto.FirstName.Trim();
        student.LastName = dto.LastName.Trim();
        student.DateOfBirth = dto.DateOfBirth;
        student.Phone = dto.Phone?.Trim() ?? string.Empty;
        student.Email = dto.Email.Trim();

        await studentRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(string id)
    {
        var student = await studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Student not found.");
        }

        studentRepository.Remove(student);
        await studentRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<int>> BulkCreateAsync(List<StudentCreateRequestDto> dtos)
    {
        if (dtos.Count == 0)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Request body cannot be empty.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var students = new List<Student>();
            foreach (var dto in dtos)
            {
                var studentId = string.IsNullOrWhiteSpace(dto.Id) ? Guid.NewGuid().ToString("N") : dto.Id.Trim();
                if (await studentRepository.ExistsAsync(studentId) || students.Any(s => s.Id == studentId))
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<int>.Fail(ServiceErrorType.Conflict, $"Duplicate student id: {studentId}");
                }

                students.Add(new Student
                {
                    Id = studentId,
                    FirstName = dto.FirstName.Trim(),
                    LastName = dto.LastName.Trim(),
                    DateOfBirth = dto.DateOfBirth,
                    Phone = dto.Phone?.Trim() ?? string.Empty,
                    Email = dto.Email.Trim()
                });
            }

            await studentRepository.AddRangeAsync(students);
            var inserted = await studentRepository.SaveChangesAsync();
            await transaction.CommitAsync();
            return ServiceResult<int>.Ok(inserted);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<StudentWithCoursesDto>> GetWithCoursesAsync()
    {
        var students = await studentRepository.GetWithCoursesAsync();
        return students.Select(s => new StudentWithCoursesDto
        {
            Id = s.Id,
            FullName = $"{s.FirstName} {s.LastName}".Trim(),
            Email = s.Email,
            Courses = s.Enrollments.Select(e => new StudentCourseDto
            {
                CourseId = e.CourseId,
                CourseName = e.Course.Name,
                EnrolledDate = e.EnrolledDate
            }).ToList()
        }).ToList();
    }

    public Task<int> CountAsync() => studentRepository.CountAsync();

    public async Task<List<StudentFullDetailsDto>> GetFullDetailsAsync()
    {
        var students = await studentRepository.GetFullDetailsAsync();
        return students.Select(s => new StudentFullDetailsDto
        {
            Id = s.Id,
            FullName = $"{s.FirstName} {s.LastName}".Trim(),
            Email = s.Email,
            Courses = s.Enrollments.Select(e => new StudentCourseFullDto
            {
                CourseId = e.CourseId,
                CourseName = e.Course.Name,
                Modules = e.Course.Modules.Select(m => new StudentModuleDto
                {
                    ModuleId = m.Id,
                    Title = m.Title,
                    Credits = m.Credits
                }).ToList()
            }).ToList()
        }).ToList();
    }

    private static StudentResponseDto MapStudent(Student student)
    {
        return new StudentResponseDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            DateOfBirth = student.DateOfBirth,
            Phone = student.Phone,
            Email = student.Email
        };
    }
}
