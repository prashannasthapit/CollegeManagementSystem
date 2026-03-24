using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModuleInstructorsController(IModuleInstructorService moduleInstructorService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Assign([FromBody] ModuleInstructorCreateRequestDto dto)
    {
        var result = await moduleInstructorService.AssignAsync(dto);
        return this.ToActionResult(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Remove([FromQuery] int moduleId, [FromQuery] int instructorId)
    {
        var result = await moduleInstructorService.RemoveAsync(moduleId, instructorId);
        return this.ToActionResult(result);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkAssign([FromBody] List<ModuleInstructorCreateRequestDto> dtos)
    {
        var result = await moduleInstructorService.BulkAssignAsync(dtos);
        return this.ToActionResult(result);
    }

    [HttpGet("full-details")]
    public async Task<ActionResult<List<ModuleInstructorDetailsDto>>> FullDetails()
    {
        var data = await moduleInstructorService.GetFullDetailsAsync();
        return Ok(data);
    }

    [HttpGet("count")]
    public async Task<ActionResult<object>> Count()
    {
        var count = await moduleInstructorService.CountAsync();
        return Ok(new { count });
    }

    [HttpGet("module-count")]
    public async Task<ActionResult<List<InstructorAssignmentCountDto>>> ModuleCount()
    {
        var data = await moduleInstructorService.GetInstructorModuleCountsAsync();
        return Ok(data);
    }
}
