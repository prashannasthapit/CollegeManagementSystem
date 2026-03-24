using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Common;

namespace CollegeManagementSystem.Services.Interfaces;

public interface IModuleInstructorService
{
    Task<ServiceResult> AssignAsync(ModuleInstructorCreateRequestDto dto);
    Task<ServiceResult> RemoveAsync(int moduleId, int instructorId);
    Task<ServiceResult<int>> BulkAssignAsync(List<ModuleInstructorCreateRequestDto> dtos);
    Task<List<ModuleInstructorDetailsDto>> GetFullDetailsAsync();
    Task<int> CountAsync();
    Task<List<InstructorAssignmentCountDto>> GetInstructorModuleCountsAsync();
}
