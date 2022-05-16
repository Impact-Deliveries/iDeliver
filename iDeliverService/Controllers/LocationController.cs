using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/Location")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _repository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public LocationController(ILocationRepository repository
            , Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
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

        [HttpPost("Addlocation")]
        public async Task<ActionResult<IEnumerable<Location>>> Addlocation([FromBody] LocationDTO model)
        {
            try {
                if (model.Id !=null && model.Id > 0)
                {
                    var location = await _repository.GetByID(model.Id.Value);
                    if (location != null) {
                        location.ModifiedDate = DateTime.UtcNow;
                        location.City = model.City;
                        location.Address = model.Address;
                        _repository.Update(location);
                    }
                }
                else
                {
                    var location = new Location()
                    {
                        Address = model.Address,
                        CountryId = 1,
                        City = model.City,
                        ModifiedDate = DateTime.UtcNow,
                        CreationDate = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _repository.Add(location);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("DeleteLocation")]
        public async Task<ActionResult> DeleteLocation([FromBody] long id)
        {

            try
            {
                Location location = await _repository.GetByID(id);
                if (location == null) return BadRequest("Location Not Found");

                location.IsDeleted = true;
                await _repository.Update(location);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        private bool LocationExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
