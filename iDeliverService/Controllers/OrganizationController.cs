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
        public OrganizationController(IOrganizationRepository repository)
        {
            _repository = repository;
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
