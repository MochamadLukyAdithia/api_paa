using Microsoft.AspNetCore.Mvc;
using praktimupaa2.Models.Student;
using System.Collections.Generic;
namespace praktimupaa2.Controllers
{
    [Route("luky/v1/[controller]")]

    public class StudentController : ControllerBase
    {
        private readonly string _consStr;

        public StudentController(IConfiguration configuration)
        {
            _consStr = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public ActionResult<Student> GetStudents()
        {
            StudentContext context = new StudentContext(_consStr);
            List<Student> students = context.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetStudentById(int id)
        {
            StudentContext context = new StudentContext(_consStr);
            Student student = context.GetStudentById(id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public ActionResult CreateStudent([FromBody] Student student)
        {
            StudentContext context = new StudentContext(_consStr);
            bool success = context.AddStudent(student);
            if (success)
                return CreatedAtAction(nameof(GetStudentById), new { id = student.id_student }, student);
            return BadRequest("Failed to create student");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.id_student)
                return BadRequest("ID mismatch");

            StudentContext context = new StudentContext(_consStr);
            bool success = context.UpdateStudent(student);
            if (success)
                return Ok(new { message = $"Update student dengan id {id} berhasil" });
            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(int id)
        {
            StudentContext context = new StudentContext(_consStr);
            bool success = context.DeleteStudent(id);
            if (success)
                return Ok(new {message = $"Menghapus student dengan id {id} berhasil"});
            return NotFound();
        }
    }
}
