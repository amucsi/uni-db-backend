using ASPNET_RESTAPI.DAL;
using ASPNET_RESTAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_RESTAPI.Controllers {
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase {
        private readonly CourseRepository courseRepository;

        public CoursesController(CourseRepository courseRepository) {
            this.courseRepository = courseRepository;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllAsync() {
            return Ok(await courseRepository.ListAsync());
        }

        [HttpGet("{id}")]
        public ActionResult<Course> GetCourseById(int id) {
            if (courseRepository.TryGetCourseById(id, out var course))
                return Ok(course);
            else
                return NotFound();
        }

        [HttpPost("add")]
        public ActionResult AddCourse([FromBody] Course course) {
            if (!courseRepository.AddCourse(course)) return BadRequest();

            return Ok(course);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCourseById(int id) {
            if (!courseRepository.DeleteCourse(id))
                return NotFound();

            return NoContent();
        }
    }
}