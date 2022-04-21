using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/MerchantBranch")]
    [ApiController]
    [Authorize]
    public class MerchantBranchController : ControllerBase
    {
        private readonly IMerchantBranchRepository _repository;

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public MerchantBranchController(IMerchantBranchRepository repository, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        // GET: api/MerchantBranch
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MerchantBranch>>> GetAllMerchantBranchs(bool IsActive=true)
        {
            var result = await _repository.Find(a=>a.IsActive==IsActive);
            return Ok(result);
        }


        // GET: api/MerchantBranch/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MerchantBranch>> GetMerchantBranch(long id)
        {
            var MerchantBranch = await _repository.GetBrancheID(id);

            if (MerchantBranch == null)
            {
                return NotFound();
            }
            return Ok(MerchantBranch);
        }
        // GET: api/GetBranchesByMerchantID/5
        [HttpGet("GetBranchesByMerchantID")]
        public async Task<ActionResult<MerchantBranch>> GetBranchesByMerchantID(long? MerchantID,long? LocationID,bool? IsActive)
        {
            var MerchantBranch = await _repository.GetBranchesByMerchantID(MerchantID ,  IsActive, LocationID);
            if (MerchantBranch == null)
            {
                return NotFound();
            }
            return Ok(MerchantBranch);
        }

        [HttpPost("AddMerchantBranch")]
        public async Task<ActionResult<MerchantBranchDTO>> AddMerchantBranch(MerchantBranchDTO modal)
        {
            try
            {
                MerchantBranch MerchantBranch = new MerchantBranch();
                if (modal.Id != null && modal.Id > 0)
                {
                     MerchantBranch = await _repository.GetByID(modal.Id.Value);
                    if (MerchantBranch != null)
                    {
                        MerchantBranch.ModifiedDate = DateTime.UtcNow;
                        MerchantBranch.Phone = modal.Phone;
                        MerchantBranch.Overview = modal.Overview;
                        MerchantBranch.Mobile = modal.Mobile;
                        MerchantBranch.Address = modal.Address;
                        MerchantBranch.BranchName = modal.BranchName;
                        MerchantBranch.Latitude = modal.Latitude;
                        MerchantBranch.Longitude = modal.Longitude;
                        MerchantBranch.MerchantId = modal.MerchantId.Value;
                        MerchantBranch.BranchPicture = modal.BranchPicture;
                        MerchantBranch.LocationId = modal.LocationId.Value;
                        await _repository.Update(MerchantBranch);
                    }


                }
                else
                {
                     MerchantBranch = new MerchantBranch()
                    {
                        ModifiedDate = DateTime.UtcNow,
                        Phone = modal.Phone,
                        Overview = modal.Overview,
                        Mobile = modal.Mobile,
                        IsActive = true,
                        CreationDate = DateTime.UtcNow,
                        Address= modal.Address,
                        BranchName = modal.BranchName,  
                        Latitude= modal.Latitude,
                        Longitude= modal.Longitude, 
                        MerchantId = modal.MerchantId.Value,
                        BranchPicture= modal.BranchPicture,
                        LocationId = modal.LocationId.Value, 
                    };
                    await _repository.Add(MerchantBranch);
                }
                return Ok(MerchantBranch);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }

        private bool MerchantBranchExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
