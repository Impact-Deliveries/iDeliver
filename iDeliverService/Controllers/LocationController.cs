using iDeliverDataAccess.Repositories;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/Location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _repository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public LocationController(ILocationRepository repository
            ,Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        // GET: api/Location
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllLocations()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }
        [HttpGet("GetLocations")]
        public async Task<ActionResult<IEnumerable<Location>>> GetActiveLocations()
        {
            var result = await _repository.Find(a => a.IsDeleted == false);
            return Ok(result);
        }

        // GET: api/Location/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(long id)
        {
            var Location = await _repository.GetByID(id);

            if (Location == null)
            {
                return NotFound();
            }
            return Ok(Location);
        }
   

       private bool LocationExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
