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
        public async Task<ActionResult<IEnumerable<MerchantBranch>>> GetAllMerchantBranchs(bool IsActive = true)
        {
         
            try
            {
                var result = await _repository.Find(a => a.IsActive == IsActive);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


        // GET: api/MerchantBranch/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MerchantBranch>> GetMerchantBranch(long id)
        {
            try {
                var MerchantBranch = await _repository.GetBrancheID(id);

                if (MerchantBranch == null)
                {
                    return NotFound();
                }
                return Ok(MerchantBranch);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
           
        }
        // GET: api/GetBranchesByMerchantID/5
        [HttpGet("GetBranchesByMerchantID")]
        public async Task<ActionResult<MerchantBranch>> GetBranchesByMerchantID(long? MerchantID, long? LocationID, bool? IsActive)
        {
            try
            {
                var MerchantBranch = await _repository.GetBranchesByMerchantID(MerchantID, IsActive, LocationID);
                if (MerchantBranch == null)
                {
                    return NotFound();
                }
                return Ok(MerchantBranch);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

           
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
                        MerchantBranch.DeliveryStatus = modal.DeliveryStatus.Value;

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
                        Address = modal.Address,
                        BranchName = modal.BranchName,
                        Latitude = modal.Latitude,
                        Longitude = modal.Longitude,
                        MerchantId = modal.MerchantId.Value,
                        BranchPicture = modal.BranchPicture,
                        LocationId = modal.LocationId.Value,
                        DeliveryStatus = modal.DeliveryStatus.Value,
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

        [HttpPost, Route("ActiveMerchantBranch")]
        public async Task<ActionResult> ActiveMerchantBranch([FromBody] long ID)
        {

            try
            {
                MerchantBranch merchantBranch = await _repository.GetByID(ID);
                if (merchantBranch == null) return BadRequest("Driver Not Found");

                merchantBranch.IsActive = !merchantBranch.IsActive;
                await _repository.Update(merchantBranch);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        private bool MerchantBranchExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
