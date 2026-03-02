using CollegeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private static readonly List<Student> Students = [];

    [HttpGet("getAll")]
    public ActionResult<List<Student>> GetStudents() => Students;

    [HttpGet("{id}")]
    public ActionResult<Student> GetStudent(string id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return NotFound();
        return student;
    }

    [HttpPost("add")]
    public ActionResult AddStudent(Student student)
    {
        if (Students.Any(s => s.Id == student.Id))
            return BadRequest("Student with this ID already exists.");
        Students.Add(student);
        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
    }

    [HttpPut("update/{id}")]
    public ActionResult UpdateStudent(string id, Student updatedStudent)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return NotFound();
        student.Name = updatedStudent.Name;
        student.Age = updatedStudent.Age;
        student.Course = updatedStudent.Course;
        return NoContent();
    }

    [HttpDelete("delete/{id}")]
    public ActionResult DeleteStudent(string id)
    {
        var student = Students.FirstOrDefault(s => s.Id == id);
        if (student == null) return NotFound();
        Students.Remove(student);
        return NoContent();
    }
}