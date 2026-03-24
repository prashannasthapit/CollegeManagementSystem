using CollegeManagementSystem.Dtos;
using CollegeManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<EnrollmentResponseDto>>> GetAll()
    {
        var data = await enrollmentService.GetAllAsync();
        return Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnrollmentCreateRequestDto dto)
    {
        var result = await enrollmentService.CreateAsync(dto);
        return this.ToActionResult(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string studentId, [FromQuery] int courseId)
    {
        var result = await enrollmentService.DeleteAsync(studentId, courseId);
        return this.ToActionResult(result);
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> BulkCreate([FromBody] List<EnrollmentCreateRequestDto> dtos)
    {
        var result = await enrollmentService.BulkCreateAsync(dtos);
        return this.ToActionResult(result);
    }

    [HttpGet("full-details")]
    public async Task<ActionResult<List<EnrollmentFullDetailsDto>>> FullDetails()
    {
        var data = await enrollmentService.GetFullDetailsAsync();
        return Ok(data);
    }

    [HttpGet("count")]
    public async Task<ActionResult<object>> Count()
    {
        var count = await enrollmentService.CountAsync();
        return Ok(new { count });
    }

    [HttpGet("by-date")]
    public async Task<ActionResult<List<EnrollmentFullDetailsDto>>> ByDate([FromQuery] DateTime date)
    {
        var data = await enrollmentService.GetByDateAsync(date);
        return Ok(data);
    }
}
