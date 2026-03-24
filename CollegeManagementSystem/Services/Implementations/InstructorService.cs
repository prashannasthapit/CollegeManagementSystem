using CollegeManagementSystem.Data.Entities;
using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Repositories.Interfaces;
using CollegeManagementSystem.Services.Common;
using CollegeManagementSystem.Services.Interfaces;
using CollegeManagementSystem.Data;

namespace CollegeManagementSystem.Services.Implementations;

public class InstructorService(IInstructorRepository instructorRepository, AppDbContext dbContext) : IInstructorService
{
    public async Task<List<InstructorResponseDto>> GetAllAsync()
    {
        var instructors = await instructorRepository.GetAllAsync();
        return instructors.Select(MapInstructor).ToList();
    }

    public async Task<InstructorResponseDto?> GetByIdAsync(int id)
    {
        var instructor = await instructorRepository.GetByIdAsync(id);
        return instructor is null ? null : MapInstructor(instructor);
    }

    public async Task<ServiceResult<InstructorResponseDto>> CreateAsync(InstructorCreateRequestDto dto)
    {
        var instructor = new Instructor
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim(),
            HireDate = dto.HireDate
        };

        await instructorRepository.AddAsync(instructor);
        await instructorRepository.SaveChangesAsync();
        return ServiceResult<InstructorResponseDto>.Ok(MapInstructor(instructor));
    }

    public async Task<ServiceResult> UpdateAsync(int id, InstructorUpdateRequestDto dto)
    {
        var instructor = await instructorRepository.GetByIdAsync(id);
        if (instructor is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Instructor not found.");
        }

        instructor.FirstName = dto.FirstName.Trim();
        instructor.LastName = dto.LastName.Trim();
        instructor.Email = dto.Email.Trim();
        instructor.HireDate = dto.HireDate;

        await instructorRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var instructor = await instructorRepository.GetByIdAsync(id);
        if (instructor is null)
        {
            return ServiceResult.Fail(ServiceErrorType.NotFound, "Instructor not found.");
        }

        instructorRepository.Remove(instructor);
        await instructorRepository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<int>> BulkCreateAsync(List<InstructorCreateRequestDto> dtos)
    {
        if (dtos.Count == 0)
        {
            return ServiceResult<int>.Fail(ServiceErrorType.BadRequest, "Request body cannot be empty.");
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var instructors = dtos.Select(d => new Instructor
            {
                FirstName = d.FirstName.Trim(),
                LastName = d.LastName.Trim(),
                Email = d.Email.Trim(),
                HireDate = d.HireDate
            }).ToList();

            await instructorRepository.AddRangeAsync(instructors);
            var inserted = await instructorRepository.SaveChangesAsync();
            await transaction.CommitAsync();
            return ServiceResult<int>.Ok(inserted);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<InstructorWithModulesDto>> GetWithModulesAsync()
    {
        var instructors = await instructorRepository.GetWithModulesAsync();
        return instructors.Select(i => new InstructorWithModulesDto
        {
            InstructorId = i.Id,
            FullName = $"{i.FirstName} {i.LastName}".Trim(),
            Modules = i.ModuleInstructors.Select(mi => new InstructorModuleDto
            {
                ModuleId = mi.ModuleId,
                Title = mi.Module.Title,
                Credits = mi.Module.Credits
            }).ToList()
        }).ToList();
    }

    public Task<int> CountAsync() => instructorRepository.CountAsync();

    public Task<List<int>> GetDistinctHireYearsAsync() => instructorRepository.GetDistinctHireYearsAsync();

    public async Task<List<InstructorModuleCountDto>> GetModuleCountsAsync()
    {
        var data = await instructorRepository.GetModuleCountsAsync();
        return data.Select(x => new InstructorModuleCountDto
        {
            InstructorId = x.instructor.Id,
            FullName = $"{x.instructor.FirstName} {x.instructor.LastName}".Trim(),
            ModuleCount = x.moduleCount
        }).ToList();
    }

    private static InstructorResponseDto MapInstructor(Instructor instructor)
    {
        return new InstructorResponseDto
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName,
            LastName = instructor.LastName,
            Email = instructor.Email,
            HireDate = instructor.HireDate
        };
    }
}
