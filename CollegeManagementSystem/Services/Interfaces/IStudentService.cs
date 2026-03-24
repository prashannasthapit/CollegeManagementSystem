using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Common;

namespace CollegeManagementSystem.Services.Interfaces;

public interface IStudentService
{
    Task<List<StudentResponseDto>> GetAllAsync();
    Task<StudentResponseDto?> GetByIdAsync(string id);
    Task<List<StudentCourseDto>?> GetCoursesAsync(string id);
    Task<ServiceResult<StudentResponseDto>> CreateAsync(StudentCreateRequestDto dto);
    Task<ServiceResult> UpdateAsync(string id, StudentUpdateRequestDto dto);
    Task<ServiceResult> DeleteAsync(string id);
    Task<ServiceResult<int>> BulkCreateAsync(List<StudentCreateRequestDto> dtos);
    Task<List<StudentWithCoursesDto>> GetWithCoursesAsync();
    Task<int> CountAsync();
    Task<List<StudentFullDetailsDto>> GetFullDetailsAsync();
}
