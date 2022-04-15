using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/MerchantBranch")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<MerchantBranch>>> GetAllMerchantBranchs()
        {
            var result = await _repository.GetAll();
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


        // GET: api/MerchantBranch/GetMerchantBranchs
        //[HttpGet, Route("GetMerchantBranchs")]
        //public async Task<ActionResult<MerchantBranch>> GetMerchantBranchs([FromQuery] NgTableParam<NgMerchantBranchTableFilter> request)
        //{
        //    try
        //    {

        //        #region Varaibles
        //        NgTableResult<MerchantBranch> results = new NgTableResult<MerchantBranch>();
        //        int total = 0;
        //        var page_index = request.page == 0 ? request.page : request.page - 1;
        //        var page_skips = page_index * request.count;
        //        NgMerchantBranchTableFilter objects = request.objects;
        //        objects = new NgMerchantBranchTableFilter()
        //        {
        //            MerchantBranchID = 0,
        //            IsActive = 1,
        //            MerchantBranchName = "",
        //            Mobile = 0,
        //        };
        //        bool isactive = request.objects != null ? (objects.IsActive == 0 ? false : true) : false;
        //        #endregion

        //        #region return results
        //        var MerchantBranchs = await _repository.GetMerchantBranchs(objects, page_skips, request.count);
        //        results = new NgTableResult<MerchantBranch>()
        //        {
        //            results = MerchantBranchs.MerchantBranchs,
        //            total = MerchantBranchs.Total
        //        };
        //        return Ok(results);
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        private bool MerchantBranchExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
