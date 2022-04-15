using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/Merchant")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantRepository _repository;

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public MerchantController(IMerchantRepository repository, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        // GET: api/Merchant
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Merchant>>> GetAllMerchants()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }
        [HttpGet("GetMerchants")]
        public async Task<ActionResult<IEnumerable<Merchant>>> GetMerchants([FromQuery] string? MerchantName,
            [FromQuery]bool? IsActive )
        {
            var result = await _repository.GetMerchants(MerchantName, IsActive);
            return Ok(result);
        }

        // GET: api/Merchant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Merchant>> GetMerchant(long id)
        {
            var Merchant = await _repository.GetByID(id);

            if (Merchant == null)
            {
                return NotFound();
            }
            return Ok(Merchant);
        }

        [HttpPost("Addmerchant")]
        public async Task<ActionResult<MerchantDTO>> Addmerchant(MerchantDTO modal)
        {
            try
            {
                if (modal.Id!=null  &&modal.Id > 0)
                {
                    var Merchant = await _repository.GetByID(modal.Id.Value);
                    if (Merchant != null)
                    {
                        Merchant.ModifiedDate = DateTime.UtcNow;
                        Merchant.Phone = modal.Phone;
                        Merchant.Overview = modal.Overview;
                        Merchant.MerchantName = modal.MerchantName;
                        Merchant.Email = modal.Email;
                        Merchant.Mobile = modal.Mobile;
                        Merchant.IsActive = modal.IsActive;
                        await _repository.Update(Merchant);
                    }


                }
                else
                {
                    Merchant merchant = new Merchant()
                    {
                        ModifiedDate = DateTime.UtcNow,
                        Phone = modal.Phone,
                        Overview = modal.Overview,
                        MerchantName = modal.MerchantName,
                        Email = modal.Email,
                        Mobile = modal.Mobile,
                        IsActive = true,
                        CreationDate = DateTime.UtcNow,
                        OrganizationId = modal.OrganizationId!=null? modal.OrganizationId.Value:1,
                    };
                    await _repository.Add(merchant);
                }
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        private bool MerchantExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
