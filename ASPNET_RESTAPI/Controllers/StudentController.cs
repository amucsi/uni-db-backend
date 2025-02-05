using ASPNET_RESTAPI.DAL;
using ASPNET_RESTAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_RESTAPI.Controllers {
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase {
        private readonly StudentRepository studentRepository;

        public StudentsController(StudentRepository studentRepository) {
            this.studentRepository = studentRepository;
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<StudentPreview>> GetAll() {
            return Ok(studentRepository.List());
        }

        [HttpGet("{neptun}")]
        public ActionResult<Student> GetStudentByNeptun(string neptun) {
            if (studentRepository.TryGetStudentByNeptun(neptun, out var student))
                return Ok(student);
            else
                return NotFound();
        }

        [HttpPost("add")]
        public ActionResult AddStudent([FromBody] Student student) {
            if (student == null) return BadRequest("Invalid student data");

            var success = studentRepository.AddStudent(student);
            if (!success) return BadRequest("Invalid major name"); //csak ezen hasalhat el

            if (!studentRepository.TryGetStudentNeptunByName(student.Name, out string studentNeptun)) //db generalja a neptunt
                return BadRequest("Couldn't find student in database");

            return CreatedAtAction(nameof(GetStudentByNeptun), new { neptun = studentNeptun }, student);
        }

        [HttpPost("{neptun}/courseattempt/{courseID}")]
        public ActionResult AddCourseAttempt(string neptun, int courseID, [FromBody] int grade) {
            var newAttempt = new CourseAttempt {
                Grade = grade,
            };

            var success = studentRepository.AddCourseAttempt(neptun, courseID, newAttempt);
            if (!success) return BadRequest("Could not add course attempt");

            return Ok();
        }

        [HttpDelete("{neptun}")]
        public ActionResult DeleteStudentByNeptun(string neptun) {
            if (!studentRepository.DeleteStudent(neptun))
                return NotFound();

            return NoContent();
        }
    }
}
