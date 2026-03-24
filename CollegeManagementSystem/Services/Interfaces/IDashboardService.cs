using CollegeManagementSystem.Dtos;

namespace CollegeManagementSystem.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummaryDto> GetSummaryAsync();
}
