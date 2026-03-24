using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Common;

namespace CollegeManagementSystem.Services.Interfaces;

public interface ICourseService
{
    Task<List<CourseResponseDto>> GetAllAsync();
    Task<CourseDetailsDto?> GetByIdAsync(int id);
    Task<List<CourseModuleDto>?> GetModulesAsync(int id);
    Task<List<CourseStudentDto>?> GetStudentsAsync(int id);
    Task<ServiceResult<CourseDetailsDto>> CreateAsync(CourseCreateRequestDto dto);
    Task<ServiceResult<ModuleResponseDto>> AddModuleAsync(int courseId, CourseModuleCreateDto dto);
    Task<ServiceResult> UpdateAsync(int id, CourseUpdateRequestDto dto);
    Task<ServiceResult> DeleteAsync(int id);
    Task<ServiceResult<int>> BulkCreateAsync(List<CourseCreateRequestDto> dtos);
    Task<List<CourseWithDetailsDto>> GetWithDetailsAsync();
    Task<int> CountAsync();
    Task<int> TotalCreditsAsync();
    Task<List<TopEnrolledCourseDto>> TopEnrolledAsync(int take = 5);
}
