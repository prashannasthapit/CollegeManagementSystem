using CollegeManagementSystem.Data;
using CollegeManagementSystem.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    [HttpGet("getAll")]
    public ActionResult<List<StudentDto>> GetStudents() => StudentData.Students;

    [HttpGet("{id}")]
    public ActionResult<StudentDto> GetStudent(string id)
    {
        var student = StudentData.Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return NotFound();
        return student;
    }

    [HttpPost("add")]
    public ActionResult AddStudent(StudentDto studentDto)
    {
        if (StudentData.Students.Any(s => s.Id == studentDto.Id))
            return BadRequest("Student with this ID already exists.");
        StudentData.Students.Add(studentDto);
        return CreatedAtAction(nameof(GetStudent), new { id = studentDto.Id }, studentDto);
    }

    [HttpPut("update/{id}")]
    public ActionResult UpdateStudent(string id, StudentDto updatedStudentDto)
    {
        var student = StudentData.Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return NotFound();
        student.Name = updatedStudentDto.Name;
        student.Age = updatedStudentDto.Age;
        student.Course = updatedStudentDto.Course;
        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    public ActionResult DeleteStudent(string id)
    {
        var student = StudentData.Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return NotFound();
        StudentData.Students.Remove(student);
        return NoContent();
    }
}