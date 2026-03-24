using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Repositories.Interfaces;
using CollegeManagementSystem.Services.Common;
using CollegeManagementSystem.Services.Interfaces;

namespace CollegeManagementSystem.Services.Implementations;

public class EnrollmentService(IEnrollmentRepository enrollmentRepository, AppDbContext dbContext) : IEnrollmentService
{
    public async Task<List<EnrollmentResponseDto>> GetAllAsync()
    {
        var enrollments = await enrollmentRepository.GetAllAsync();
        return enrollments.Select(MapEnrollment).ToList();
    }

    public async Task<ServiceResult<EnrollmentResponseDto>> CreateAsync(EnrollmentCreateRequestDto dto)
    {
        if (!await enrollmentRepository.StudentExistsAsync(dto.StudentId))
        {
            return ServiceResult<EnrollmentResponseDto>.Fail(ServiceErrorType.BadRequest, "Invalid StudentId.");
        }

        if (!await enrollmentRepository.CourseExistsAsync(dto.CourseId))
        {
            return ServiceResult<EnrollmentResponseDto>.Fail(ServiceErrorType.BadRequest, "Invalid CourseId.");
        }

        if (await enrollmentRepository.EnrollmentExistsAsync(dto.StudentId, dto.CourseId))
        {
            return ServiceResult<EnrollmentResponseDto>.Fail(ServiceErrorType.Conflict, "Enrollment already exists.");
        }

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId.Trim(),
            CourseId = dto.CourseId,
            EnrolledDate = dto.EnrolledDate?.Date ?? DateTime.UtcNow.Date
        };

        await enrollmentRepository.AddAsync(enrollment);
        await enrollmentRepository.SaveChangesAsync();

        return ServiceResult<EnrollmentResponseDto>.Ok(MapEnrollment(enrollment));
    }

    public async Task<ServiceResult> DeleteAsync(string studentId, int courseId)
    {
        var enrollment = await enrollmentRepository.GetByIdAsync(studentId, courseId);
        if (enrollment is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Enrollment not found.");
        }

        enrollmentRepository.Remove(enrollment);
        await enrollmentRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<int>> BulkCreateAsync(List<EnrollmentCreateRequestDto> dtos)
    {
        if (dtos.Count == 0)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Request body cannot be empty.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var enrollments = new List<Enrollment>();
            foreach (var dto in dtos)
            {
                if (!await enrollmentRepository.StudentExistsAsync(dto.StudentId) ||
                    !await enrollmentRepository.CourseExistsAsync(dto.CourseId))
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Invalid StudentId or CourseId found in request.");
                }

                if (await enrollmentRepository.EnrollmentExistsAsync(dto.StudentId, dto.CourseId) ||
                    enrollments.Any(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId))
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<int>.Fail(ServiceErrorType.Conflict, "Duplicate enrollment found in request.");
                }

                enrollments.Add(new Enrollment
                {
                    StudentId = dto.StudentId.Trim(),
                    CourseId = dto.CourseId,
                    EnrolledDate = dto.EnrolledDate?.Date ?? DateTime.UtcNow.Date
                });
            }

            await enrollmentRepository.AddRangeAsync(enrollments);
            var inserted = await enrollmentRepository.SaveChangesAsync();
            await transaction.CommitAsync();
            return ServiceResult<int>.Ok(inserted);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<EnrollmentFullDetailsDto>> GetFullDetailsAsync()
    {
        var enrollments = await enrollmentRepository.GetFullDetailsAsync();
        return enrollments.Select(e => new EnrollmentFullDetailsDto
        {
            StudentId = e.StudentId,
            StudentName = $"{e.Student.FirstName} {e.Student.LastName}".Trim(),
            CourseId = e.CourseId,
            CourseName = e.Course.Name,
            EnrolledDate = e.EnrolledDate
        }).ToList();
    }

    public Task<int> CountAsync() => enrollmentRepository.CountAsync();

    public async Task<List<EnrollmentFullDetailsDto>> GetByDateAsync(DateTime date)
    {
        var enrollments = await enrollmentRepository.GetByDateAsync(date);
        return enrollments.Select(e => new EnrollmentFullDetailsDto
        {
            StudentId = e.StudentId,
            StudentName = $"{e.Student.FirstName} {e.Student.LastName}".Trim(),
            CourseId = e.CourseId,
            CourseName = e.Course.Name,
            EnrolledDate = e.EnrolledDate
        }).ToList();
    }

    private static EnrollmentResponseDto MapEnrollment(Enrollment enrollment)
    {
        return new EnrollmentResponseDto
        {
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrolledDate = enrollment.EnrolledDate
        };
    }
}
