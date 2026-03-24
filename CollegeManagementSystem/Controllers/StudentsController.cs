using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<StudentResponseDto>>> GetAll()
    {
        var students = await studentService.GetAllAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentResponseDto>> GetById(string id)
    {
        var student = await studentService.GetByIdAsync(id);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpGet("{id}/courses")]
    public async Task<ActionResult<List<StudentCourseDto>>> GetCourses(string id)
    {
        var courses = await studentService.GetCoursesAsync(id);
        return courses is null ? NotFound() : Ok(courses);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StudentCreateRequestDto dto)
    {
        var result = await studentService.CreateAsync(dto);
        if (!result.Success)
        {
            return this.ToActionResult(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] StudentUpdateRequestDto dto)
    {
        var result = await studentService.UpdateAsync(id, dto);
        return this.ToActionResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await studentService.DeleteAsync(id);
        return this.ToActionResult(result);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] List<StudentCreateRequestDto> dtos)
    {
        var result = await studentService.BulkCreateAsync(dtos);
        return this.ToActionResult(result);
    }

    [HttpGet("with-courses")]
    public async Task<ActionResult<List<StudentWithCoursesDto>>> GetWithCourses()
    {
        var data = await studentService.GetWithCoursesAsync();
        return Ok(data);
    }

    [HttpGet("count")]
    public async Task<ActionResult<object>> Count()
    {
        var count = await studentService.CountAsync();
        return Ok(new { count });
    }

    [HttpGet("full-details")]
    public async Task<ActionResult<List<StudentFullDetailsDto>>> FullDetails()
    {
        var data = await studentService.GetFullDetailsAsync();
        return Ok(data);
    }
}
