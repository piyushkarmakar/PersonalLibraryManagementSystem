using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Data;

using Microsoft.EntityFrameworkCore;


namespace StudentManagement.Controllers
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
                return await _context.Students.ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Student>> GetStudentById(int id)
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null) return NotFound();
                return student;
            }
        }
    


}
