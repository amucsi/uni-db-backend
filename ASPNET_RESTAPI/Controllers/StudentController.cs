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
        public async Task<ActionResult<IEnumerable<StudentPreview>>> GetAllStudentsAsync() {
            return Ok(await studentRepository.ListAsync());
        }

        [HttpGet("{neptun}")]
        public async Task<ActionResult<Student>> GetStudentByNeptunAsync(string neptun) {
            (bool success, Student? student) = await studentRepository.GetStudentByNeptunAsync(neptun);
            if (success)
                return Ok(student);
            else
                return NotFound();
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddStudentAsync([FromBody] Student student) {
            if (!await studentRepository.AddStudentAsync(student))
                return BadRequest();
            else
                return CreatedAtAction(
                    nameof(GetStudentByNeptunAsync),
                    new { neptun = student.NEPTUN },
                    student
                );
        }

        [HttpPost("{neptun}/courseattempt/{courseID}")]
        public async Task<ActionResult> AddCourseAttemptAsync(string neptun, int courseID, [FromBody] int grade) {
            var newAttempt = new CourseAttempt {
                Grade = grade,
            };

            if (!await studentRepository.AddCourseAttemptAsync(neptun, courseID, newAttempt))
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{neptun}")]
        public async Task<ActionResult> DeleteStudentByNeptunAsyncs(string neptun) {
            if (!await studentRepository.DeleteStudentAsync(neptun))
                return NotFound();
            else
                return NoContent();
        }
    }
}
