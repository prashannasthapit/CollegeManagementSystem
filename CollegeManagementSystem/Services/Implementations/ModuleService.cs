using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Repositories.Interfaces;
using CollegeManagementSystem.Services.Common;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagementSystem.Services.Implementations;

public class ModuleService(IModuleRepository moduleRepository, AppDbContext dbContext) : IModuleService
{
    public async Task<List<ModuleResponseDto>> GetAllAsync()
    {
        var modules = await moduleRepository.GetAllAsync();
        return modules.Select(m => MapModule(m)).ToList();
    }

    public async Task<ModuleResponseDto?> GetByIdAsync(int id)
    {
        var module = await moduleRepository.GetByIdWithCourseAsync(id);
        return module is null ? null : MapModule(module, module.Course?.Name);
    }

    public async Task<List<ModuleInstructorDto>?> GetInstructorsAsync(int id)
    {
        var module = await moduleRepository.GetByIdWithInstructorsAsync(id);
        if (module is null)
        {
            return null;
        }

        return module.ModuleInstructors.Select(mi => new ModuleInstructorDto
        {
            InstructorId = mi.InstructorId,
            FullName = $"{mi.Instructor.FirstName} {mi.Instructor.LastName}".Trim(),
            Email = mi.Instructor.Email
        }).ToList();
    }

    public async Task<ServiceResult<ModuleResponseDto>> CreateAsync(ModuleCreateRequestDto dto)
    {
        var courseExists = await dbContext.Courses.AnyAsync(c => c.Id == dto.CourseId);
        if (!courseExists)
        {
            return ServiceResult<ModuleResponseDto>.Fail(ServiceErrorType.BadRequest, "Invalid CourseId.");
        }

        var module = new Module
        {
            Title = dto.Title.Trim(),
            Credits = dto.Credits,
            CourseId = dto.CourseId
        };

        await moduleRepository.AddAsync(module);
        await moduleRepository.SaveChangesAsync();

        return ServiceResult<ModuleResponseDto>.Ok(MapModule(module));
    }

    public async Task<ServiceResult> UpdateAsync(int id, ModuleUpdateRequestDto dto)
    {
        var module = await moduleRepository.GetByIdAsync(id);
        if (module is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Module not found.");
        }

        var courseExists = await dbContext.Courses.AnyAsync(c => c.Id == dto.CourseId);
        if (!courseExists)
        {
            return ServiceResult.Fail(ServiceErrorType.BadRequest, "Invalid CourseId.");
        }

        module.Title = dto.Title.Trim();
        module.Credits = dto.Credits;
        module.CourseId = dto.CourseId;

        await moduleRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var module = await moduleRepository.GetByIdAsync(id);
        if (module is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Module not found.");
        }

        moduleRepository.Remove(module);
        await moduleRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<int>> BulkCreateAsync(List<ModuleCreateRequestDto> dtos)
    {
        if (dtos.Count == 0)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Request body cannot be empty.");
        }

        var courseIds = dtos.Select(d => d.CourseId).Distinct().ToList();
        var existingCourseIds = await dbContext.Courses
            .Where(c => courseIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();

        if (existingCourseIds.Count != courseIds.Count)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "One or more CourseId values are invalid.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var modules = dtos.Select(d => new Module
            {
                Title = d.Title.Trim(),
                Credits = d.Credits,
                CourseId = d.CourseId
            }).ToList();

            await moduleRepository.AddRangeAsync(modules);
            var inserted = await moduleRepository.SaveChangesAsync();
            await transaction.CommitAsync();
            return ServiceResult<int>.Ok(inserted);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<ModuleResponseDto>> GetWithCourseAsync()
    {
        var modules = await moduleRepository.GetWithCourseAsync();
        return modules.Select(m => MapModule(m, m.Course.Name)).ToList();
    }

    public Task<int> CountAsync() => moduleRepository.CountAsync();

    public async Task<List<ModuleResponseDto>> GetHighCreditAsync(int minCredits)
    {
        var modules = await moduleRepository.GetHighCreditAsync(minCredits);
        return modules.Select(m => MapModule(m)).ToList();
    }

    public async Task<ServiceResult<int>> BulkUpdateCreditsAsync(BulkUpdateModuleCreditsRequestDto dto)
    {
        if (dto.Updates.Count == 0)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Updates cannot be empty.");
        }

        var updatesById = dto.Updates.GroupBy(u => u.ModuleId).ToDictionary(g => g.Key, g => g.Last());
        var modules = await moduleRepository.GetByIdsAsync(updatesById.Keys);

        if (modules.Count != updatesById.Count)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "One or more ModuleId values are invalid.");
        }

        foreach (var module in modules)
        {
            module.Credits = updatesById[module.Id].Credits;
        }

        var updated = await moduleRepository.SaveChangesAsync();
        return ServiceResult<int>.Ok(updated);
    }

    private static ModuleResponseDto MapModule(Module module, string? courseName = null)
    {
        return new ModuleResponseDto
        {
            Id = module.Id,
            Title = module.Title,
            Credits = module.Credits,
            CourseId = module.CourseId,
            CourseName = courseName
        };
    }
}
