using ASPNET_RESTAPI.DAL;
using ASPNET_RESTAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace ASPNET_RESTAPI.Controllers {
    [Route("api/[controller]")]
    public class MajorsController : ControllerBase {
        private readonly MajorRepository majorRepository;
        
        public MajorsController(MajorRepository majorRepository) {
            this.majorRepository = majorRepository;
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<Major>> GetAllMajors() {
            return Ok(majorRepository.List());
        }

        [HttpGet("{id}")]
        public ActionResult<Major> GetMajorById(int id) {
            if (majorRepository.TryGetMajorById(id,  out var major)) 
                return Ok(major);
            else
                return NotFound();
        }

        [HttpPost("add")]
        public ActionResult AddMajor([FromBody] Major major) {
            if (!majorRepository.AddMajor(major)) return BadRequest();

            return Ok(major);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMajorById(int id) {
            if (!majorRepository.DeleteMajor(id))
                return NotFound();

            return NoContent();
        }
    }
}
