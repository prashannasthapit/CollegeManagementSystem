using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(ICourseService courseService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CourseResponseDto>>> GetAll()
    {
        var courses = await courseService.GetAllAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseDetailsDto>> GetById(int id)
    {
        var course = await courseService.GetByIdAsync(id);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpGet("{id}/modules")]
    public async Task<ActionResult<List<CourseModuleDto>>> GetModules(int id)
    {
        var modules = await courseService.GetModulesAsync(id);
        return modules is null ? NotFound() : Ok(modules);
    }

    [HttpGet("{id}/students")]
    public async Task<ActionResult<List<CourseStudentDto>>> GetStudents(int id)
    {
        var students = await courseService.GetStudentsAsync(id);
        return students is null ? NotFound() : Ok(students);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CourseCreateRequestDto dto)
    {
        var result = await courseService.CreateAsync(dto);
        if (!result.Success)
        {
            return this.ToActionResult(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPost("{id}/modules")]
    public async Task<IActionResult> AddModule(int id, [FromBody] CourseModuleCreateDto dto)
    {
        var result = await courseService.AddModuleAsync(id, dto);
        return this.ToActionResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CourseUpdateRequestDto dto)
    {
        var result = await courseService.UpdateAsync(id, dto);
        return this.ToActionResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await courseService.DeleteAsync(id);
        return this.ToActionResult(result);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] List<CourseCreateRequestDto> dtos)
    {
        var result = await courseService.BulkCreateAsync(dtos);
        return this.ToActionResult(result);
    }

    [HttpGet("with-details")]
    public async Task<ActionResult<List<CourseWithDetailsDto>>> GetWithDetails()
    {
        var data = await courseService.GetWithDetailsAsync();
        return Ok(data);
    }

    [HttpGet("count")]
    public async Task<ActionResult<object>> Count()
    {
        var count = await courseService.CountAsync();
        return Ok(new { count });
    }

    [HttpGet("total-credits")]
    public async Task<ActionResult<object>> TotalCredits()
    {
        var totalCredits = await courseService.TotalCreditsAsync();
        return Ok(new { totalCredits });
    }

    [HttpGet("top-enrolled")]
    public async Task<ActionResult<List<TopEnrolledCourseDto>>> TopEnrolled([FromQuery] int take = 5)
    {
        var data = await courseService.TopEnrolledAsync(take);
        return Ok(data);
    }
}
