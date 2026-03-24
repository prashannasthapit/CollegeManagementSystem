using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Common;

namespace CollegeManagementSystem.Services.Interfaces;

public interface IInstructorService
{
    Task<List<InstructorResponseDto>> GetAllAsync();
    Task<InstructorResponseDto?> GetByIdAsync(int id);
    Task<ServiceResult<InstructorResponseDto>> CreateAsync(InstructorCreateRequestDto dto);
    Task<ServiceResult> UpdateAsync(int id, InstructorUpdateRequestDto dto);
    Task<ServiceResult> DeleteAsync(int id);
    Task<ServiceResult<int>> BulkCreateAsync(List<InstructorCreateRequestDto> dtos);
    Task<List<InstructorWithModulesDto>> GetWithModulesAsync();
    Task<int> CountAsync();
    Task<List<int>> GetDistinctHireYearsAsync();
    Task<List<InstructorModuleCountDto>> GetModuleCountsAsync();
}
