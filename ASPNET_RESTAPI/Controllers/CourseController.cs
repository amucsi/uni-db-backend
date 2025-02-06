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
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCoursesAsync() {
            return Ok(await courseRepository.ListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseByIdAsync(int id) {
            (bool success, Course? course) = await courseRepository.GetCourseByIdAsync(id);
            if (success)
                return Ok(course);
            else
                return NotFound();
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCourseAsync([FromBody] Course course) {
            if (!await courseRepository.AddCourseAsync(course))
                return BadRequest();
            else
                return CreatedAtAction(
                    nameof(GetCourseByIdAsync),
                    new { id = course.ID },
                    course
                );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourseByIdAsync(int id) {
            if (!await courseRepository.DeleteCourseAsync(id))
                return NotFound();
            else
                return NoContent();
        }
    }
}