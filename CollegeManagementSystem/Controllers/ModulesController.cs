using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModulesController(IModuleService moduleService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ModuleResponseDto>>> GetAll()
    {
        var modules = await moduleService.GetAllAsync();
        return Ok(modules);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ModuleResponseDto>> GetById(int id)
    {
        var module = await moduleService.GetByIdAsync(id);
        return module is null ? NotFound() : Ok(module);
    }

    [HttpGet("{id}/instructors")]
    public async Task<ActionResult<List<ModuleInstructorDto>>> GetInstructors(int id)
    {
        var instructors = await moduleService.GetInstructorsAsync(id);
        return instructors is null ? NotFound() : Ok(instructors);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ModuleCreateRequestDto dto)
    {
        var result = await moduleService.CreateAsync(dto);
        return this.ToActionResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ModuleUpdateRequestDto dto)
    {
        var result = await moduleService.UpdateAsync(id, dto);
        return this.ToActionResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await moduleService.DeleteAsync(id);
        return this.ToActionResult(result);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] List<ModuleCreateRequestDto> dtos)
    {
        var result = await moduleService.BulkCreateAsync(dtos);
        return this.ToActionResult(result);
    }

    [HttpGet("with-course")]
    public async Task<ActionResult<List<ModuleResponseDto>>> GetWithCourse()
    {
        var modules = await moduleService.GetWithCourseAsync();
        return Ok(modules);
    }

    [HttpGet("count")]
    public async Task<ActionResult<object>> Count()
    {
        var count = await moduleService.CountAsync();
        return Ok(new { count });
    }

    [HttpGet("high-credit")]
    public async Task<ActionResult<List<ModuleResponseDto>>> HighCredit([FromQuery] int minCredits)
    {
        var modules = await moduleService.GetHighCreditAsync(minCredits);
        return Ok(modules);
    }

    [HttpPut("bulk-update-credits")]
    public async Task<IActionResult> BulkUpdateCredits([FromBody] BulkUpdateModuleCreditsRequestDto dto)
    {
        var result = await moduleService.BulkUpdateCreditsAsync(dto);
        return this.ToActionResult(result);
    }
}
