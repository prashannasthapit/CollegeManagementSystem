using CollegeManagementSystem.Data;
using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Repositories.Interfaces;
using CollegeManagementSystem.Services.Common;
using CollegeManagementSystem.Services.Interfaces;

namespace CollegeManagementSystem.Services.Implementations;

public class ModuleInstructorService(IModuleInstructorRepository moduleInstructorRepository, AppDbContext dbContext) : IModuleInstructorService
{
    public async Task<ServiceResult> AssignAsync(ModuleInstructorCreateRequestDto dto)
    {
        if (!await moduleInstructorRepository.ModuleExistsAsync(dto.ModuleId))
        {
            return ServiceResult.Fail(ServiceErrorType.BadRequest, "Invalid ModuleId.");
        }

        if (!await moduleInstructorRepository.InstructorExistsAsync(dto.InstructorId))
        {
            return ServiceResult.Fail(ServiceErrorType.BadRequest, "Invalid InstructorId.");
        }

        if (await moduleInstructorRepository.AssignmentExistsAsync(dto.ModuleId, dto.InstructorId))
        {
            return ServiceResult.Fail(ServiceErrorType.Conflict, "Assignment already exists.");
        }

        await moduleInstructorRepository.AddAsync(new ModuleInstructor
        {
            ModuleId = dto.ModuleId,
            InstructorId = dto.InstructorId
        });

        await moduleInstructorRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> RemoveAsync(int moduleId, int instructorId)
    {
        var assignment = await moduleInstructorRepository.GetByIdAsync(moduleId, instructorId);
        if (assignment is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Assignment not found.");
        }

        moduleInstructorRepository.Remove(assignment);
        await moduleInstructorRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<int>> BulkAssignAsync(List<ModuleInstructorCreateRequestDto> dtos)
    {
        if (dtos.Count == 0)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Request body cannot be empty.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var assignments = new List<ModuleInstructor>();
            foreach (var dto in dtos)
            {
                if (!await moduleInstructorRepository.ModuleExistsAsync(dto.ModuleId) ||
                    !await moduleInstructorRepository.InstructorExistsAsync(dto.InstructorId))
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Invalid ModuleId or InstructorId found in request.");
                }

                if (await moduleInstructorRepository.AssignmentExistsAsync(dto.ModuleId, dto.InstructorId) ||
                    assignments.Any(a => a.ModuleId == dto.ModuleId && a.InstructorId == dto.InstructorId))
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<int>.Fail(ServiceErrorType.Conflict, "Duplicate assignment found in request.");
                }

                assignments.Add(new ModuleInstructor
                {
                    ModuleId = dto.ModuleId,
                    InstructorId = dto.InstructorId
                });
            }

            await moduleInstructorRepository.AddRangeAsync(assignments);
            var inserted = await moduleInstructorRepository.SaveChangesAsync();
            await transaction.CommitAsync();
            return ServiceResult<int>.Ok(inserted);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<ModuleInstructorDetailsDto>> GetFullDetailsAsync()
    {
        var assignments = await moduleInstructorRepository.GetFullDetailsAsync();
        return assignments.Select(a => new ModuleInstructorDetailsDto
        {
            ModuleId = a.ModuleId,
            ModuleTitle = a.Module.Title,
            InstructorId = a.InstructorId,
            InstructorName = $"{a.Instructor.FirstName} {a.Instructor.LastName}".Trim()
        }).ToList();
    }

    public Task<int> CountAsync() => moduleInstructorRepository.CountAsync();

    public async Task<List<InstructorAssignmentCountDto>> GetInstructorModuleCountsAsync()
    {
        var data = await moduleInstructorRepository.GetInstructorAssignmentCountsAsync();
        return data.Select(x => new InstructorAssignmentCountDto
        {
            InstructorId = x.instructorId,
            InstructorName = $"{x.firstName} {x.lastName}".Trim(),
            AssignmentCount = x.assignmentCount
        }).ToList();
    }
}
