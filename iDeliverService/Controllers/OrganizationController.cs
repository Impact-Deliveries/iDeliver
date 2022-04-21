using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/Organization")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository _repository;
        private readonly IEnrolmentRepository _Erepository;
        private readonly IUserRepository _Urepository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public OrganizationController(IOrganizationRepository repository, IEnrolmentRepository Erepository,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IUserRepository Urepository)
        {
            _repository = repository;
            _Erepository = Erepository;
            _Urepository = Urepository;
            _env = env;
        }

        // GET: api/Organization
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Organization>>> GetAllOrganizations()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }
        [HttpGet("GetActiveOrNotOrganizations")]
        public async Task<ActionResult<IEnumerable<Organization>>> GetActiveOrganizations(bool IsActive = true)
        {
            var result = await _repository.Find(a => a.IsActive == IsActive);
            return Ok(result);
        }

        // GET: api/Organization/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(long id)
        {
            var Organization = await _repository.GetByID(id);

            if (Organization == null)
            {
                return NotFound();
            }
            return Ok(Organization);
        }

        private bool OrganizationExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
