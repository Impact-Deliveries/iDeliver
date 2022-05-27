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
        private readonly IDriverRepository _Drepository;
        private readonly IDriverDetailsRepository _Derepository;
        private readonly IDriverSchaduleRepository _Srepository;
        private readonly IEnrolmentRepository _Erepository;
        private readonly IUserRepository _Urepository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public DriverCaseController(IDriverCaseRepository repository,IDriverRepository Drepository, IDriverDetailsRepository Derepository,
            IDriverSchaduleRepository Srepository, IEnrolmentRepository Erepository,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IUserRepository Urepository)
        {
            _repository = repository;
            _Drepository = Drepository;
            _Derepository = Derepository;
            _Srepository = Srepository;
            _Erepository = Erepository;
            _Urepository = Urepository;
            _env = env;
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

        private bool DriverCaseExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
