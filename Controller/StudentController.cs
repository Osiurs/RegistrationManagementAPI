using Microsoft.AspNetCore.Mvc;
using RegistrationManagementAPI.Entities;
using RegistrationManagementAPI.DTOs;
using RegistrationManagementAPI.Services;
using System.Threading.Tasks;

namespace RegistrationManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "StudentId",
            [FromQuery] bool isDescending = false)
        {
            var students = await _studentService.GetStudentsPagedAsync(pageNumber, pageSize, sortBy, isDescending);
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentDTO studentDTO)
        {
            if (!ModelState.IsValid)
            {
                // Log the model validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage).ToList();
                Console.WriteLine("Model validation errors: " + string.Join(", ", errors));
                return BadRequest(ModelState);
            }

            var newStudent = await _studentService.AddStudentAsync(studentDTO);
            return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.StudentId }, newStudent);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDTO studentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedStudent = await _studentService.UpdateStudentAsync(id, studentDTO);
            if (updatedStudent == null)
            {
                return NotFound(new { message = "Student not found" });
            }
            return Ok(updatedStudent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var existingStudent = await _studentService.GetStudentByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }
    }
}
