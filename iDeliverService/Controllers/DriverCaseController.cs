using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/DriverCase")]
    [ApiController]
    [Authorize]
    public class DriverCaseController : ControllerBase
    {
        private readonly IDriverCaseRepository _repository;

        public DriverCaseController(IDriverCaseRepository repository)
        {
            _repository = repository;
        }

        // GET: api/DriverCase
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DriverCase>>> GetAllDriverCases()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }


        // GET: api/DriverCase/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverCase>> GetDriverCase(long id)
        {
            var DriverCase = await _repository.GetByID(id);

            if (DriverCase == null)
            {
                return NotFound();
            }
            return Ok(DriverCase);
        }

  
        [HttpGet, Route("GetOnlineDrivers")]
        public async Task<ActionResult<DriverCase>> GetOnlineDrivers()
        {
            try
            {
                var Drivers = await _repository.GetOnlineDrivers();
                return Ok(Drivers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("SetDriverCase")]
        public async Task<ActionResult<DriverCase>> SetDriverCase([FromBody] DriverCaseDTO _request)
        {
            try
            {
                var driverCase = await _repository.SetDriverCase(_request);
                return Ok(driverCase);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool DriverCaseExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }
    }
}
