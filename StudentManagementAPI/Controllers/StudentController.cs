using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Data;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            try
            {
                return await _context.Students.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching students", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null) return NotFound(new { Message = "Student not found" });
                return student;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching student", Details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
            }
            catch (DbUpdateException dbEx)
            {
                // Database-related errors
                return StatusCode(500, new { Message = "Database error occurred", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                // Any other errors
                return StatusCode(500, new { Message = "An error occurred while creating the student", Details = ex.Message });
            }

        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null) return NotFound(new { message = "Student not found" });

                // Update only fields sent in request
                student.Name = updatedStudent.Name ?? student.Name;
                student.Age = updatedStudent.Age;

                await _context.SaveChangesAsync();
                return Ok(student);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error occurred while updating", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the student", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null) return NotFound(new { message = "Student not found" });

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Student with ID {id} deleted successfully" });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Database error occurred while deleting", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the student", details = ex.Message });
            }
        }
    }
}
