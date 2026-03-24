using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Common;

namespace CollegeManagementSystem.Services.Interfaces;

public interface IEnrollmentService
{
    Task<List<EnrollmentResponseDto>> GetAllAsync();
    Task<ServiceResult<EnrollmentResponseDto>> CreateAsync(EnrollmentCreateRequestDto dto);
    Task<ServiceResult> DeleteAsync(string studentId, int courseId);
    Task<ServiceResult<int>> BulkCreateAsync(List<EnrollmentCreateRequestDto> dtos);
    Task<List<EnrollmentFullDetailsDto>> GetFullDetailsAsync();
    Task<int> CountAsync();
    Task<List<EnrollmentFullDetailsDto>> GetByDateAsync(DateTime date);
}
