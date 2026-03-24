using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Common;

namespace CollegeManagementSystem.Services.Interfaces;

public interface IModuleService
{
    Task<List<ModuleResponseDto>> GetAllAsync();
    Task<ModuleResponseDto?> GetByIdAsync(int id);
    Task<List<ModuleInstructorDto>?> GetInstructorsAsync(int id);
    Task<ServiceResult<ModuleResponseDto>> CreateAsync(ModuleCreateRequestDto dto);
    Task<ServiceResult> UpdateAsync(int id, ModuleUpdateRequestDto dto);
    Task<ServiceResult> DeleteAsync(int id);
    Task<ServiceResult<int>> BulkCreateAsync(List<ModuleCreateRequestDto> dtos);
    Task<List<ModuleResponseDto>> GetWithCourseAsync();
    Task<int> CountAsync();
    Task<List<ModuleResponseDto>> GetHighCreditAsync(int minCredits);
    Task<ServiceResult<int>> BulkUpdateCreditsAsync(BulkUpdateModuleCreditsRequestDto dto);
}
