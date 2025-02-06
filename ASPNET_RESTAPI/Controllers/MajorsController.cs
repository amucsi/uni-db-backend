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
        public async Task<ActionResult<IEnumerable<Major>>> GetAllMajorsAsync() {
            return Ok(await majorRepository.ListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Major>> GetMajorByIdAsync(int id) {
            (bool success, Major? major) = await majorRepository.GetMajorByIdAsync(id);
            if (success)
                return Ok(major);
            else
                return NotFound();
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddMajorAsync([FromBody] Major major) {
            if (!await majorRepository.AddMajorAsync(major))
                return BadRequest();
            else
                return CreatedAtAction(
                    nameof(GetMajorByIdAsync),
                    new { id = major.ID },
                    major
                );
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMajorByIdAsync(int id) {
            if (!await majorRepository.DeleteMajorAsync(id))
                return NotFound();
            else
                return NoContent();
        }
    }
}
