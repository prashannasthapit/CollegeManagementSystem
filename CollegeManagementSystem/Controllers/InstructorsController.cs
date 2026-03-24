using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController(IInstructorService instructorService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<InstructorResponseDto>>> GetAll()
    {
        var instructors = await instructorService.GetAllAsync();
        return Ok(instructors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InstructorResponseDto>> GetById(int id)
    {
        var instructor = await instructorService.GetByIdAsync(id);
        return instructor is null ? NotFound() : Ok(instructor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InstructorCreateRequestDto dto)
    {
        var result = await instructorService.CreateAsync(dto);
        return this.ToActionResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] InstructorUpdateRequestDto dto)
    {
        var result = await instructorService.UpdateAsync(id, dto);
        return this.ToActionResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await instructorService.DeleteAsync(id);
        return this.ToActionResult(result);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] List<InstructorCreateRequestDto> dtos)
    {
        var result = await instructorService.BulkCreateAsync(dtos);
        return this.ToActionResult(result);
    }

    [HttpGet("modules")]
    public async Task<ActionResult<List<InstructorWithModulesDto>>> Modules()
    {
        var data = await instructorService.GetWithModulesAsync();
        return Ok(data);
    }

    [HttpGet("count")]
    public async Task<ActionResult<object>> Count()
    {
        var count = await instructorService.CountAsync();
        return Ok(new { count });
    }

    [HttpGet("distinct-hireyear")]
    public async Task<ActionResult<List<int>>> DistinctHireYear()
    {
        var years = await instructorService.GetDistinctHireYearsAsync();
        return Ok(years);
    }

    [HttpGet("module-count")]
    public async Task<ActionResult<List<InstructorModuleCountDto>>> ModuleCount()
    {
        var counts = await instructorService.GetModuleCountsAsync();
        return Ok(counts);
    }
}
