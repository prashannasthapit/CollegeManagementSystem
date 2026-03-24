using CollegeManagementSystem.Data;
using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Services.Implementations;

public class DashboardService(AppDbContext dbContext) : IDashboardService
{
    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        return new DashboardSummaryDto
        {
            Students = await dbContext.Students.CountAsync(),
            Courses = await dbContext.Courses.CountAsync(),
            Modules = await dbContext.Modules.CountAsync(),
            Enrollments = await dbContext.Enrollments.CountAsync()
        };
    }
}
