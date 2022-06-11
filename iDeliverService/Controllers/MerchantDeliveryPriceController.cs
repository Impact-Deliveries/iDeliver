using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/MerchantDeliveryPrice")]
    [ApiController]
    [Authorize]
    public class MerchantDeliveryPriceController : ControllerBase
    {
        private readonly IMerchantDeliveryPriceRepository _repository;
        private readonly IMerchantBranchRepository _Brepository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public MerchantDeliveryPriceController(IMerchantDeliveryPriceRepository repository, IMerchantBranchRepository Brepository,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IUserRepository Urepository)
        {
            _repository = repository;
            _Brepository = Brepository;
            _env = env;
        }

        // GET: api/MerchantDeliveryPrice
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MerchantDeliveryPrice>>> GetAllMerchantDeliveryPrices()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }


        // GET: api/MerchantDeliveryPrice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MerchantDeliveryPrice>> GetMerchantDeliveryPrice(long id)
        {
            var MerchantDeliveryPrice = await _repository.GetByID(id);

            if (MerchantDeliveryPrice == null)
            {
                return NotFound();
            }
            return Ok(MerchantDeliveryPrice);
        }

        [HttpPost("SaveDeliveryPrices")]
        public async Task<ActionResult<MerchantDeliveryPrice>> SaveDeliveryPrices(DeliveryPriceDTO model)
        {

            try
            {
                // _repository 
                // _Brepository
                if (model.MerchantBranchID == null || model.DeliveryStatus == null
                    || (model.DeliveryPrice == null && model.DeliveryStatus != 3))
                    return BadRequest();

                var branch = await _Brepository.GetByID(model.MerchantBranchID.Value);
                if (branch == null) return NotFound();

                branch.DeliveryStatus = model.DeliveryStatus.Value;
                switch (model.DeliveryStatus.Value)
                {
                    case 1://location
                        var old_locations = await _repository.Find(a => a.MerchantBranchId == model.MerchantBranchID && a.LocationId != null);
                        if (old_locations != null && old_locations.Count() > 0)
                        {
                            foreach (var item in old_locations)
                            {
                                await _repository.Delete(item);
                            }
                        }
                        var locations = model.DeliveryPrice.Where(a => a.LocationId != null).ToList();
                        if (locations != null && locations.Count() > 0)
                        {
                            foreach (var item in locations)
                            {
                                MerchantDeliveryPrice obj = new MerchantDeliveryPrice()
                                {
                                    MerchantBranchId = model.MerchantBranchID.Value,
                                    LocationId = item.LocationId,
                                    FromDistance = null,
                                    ToDistance = null,
                                    Amount = item.Amount == null ? 0 : item.Amount.Value,
                                    IsDeleted = false,
                                    ModifiedDate = DateTime.UtcNow,
                                    CreationDate = DateTime.UtcNow,
                                };
                                await _repository.Add(obj);
                            }
                        }
                        break;
                    case 2://distance
                        var old_distance = await _repository.Find(a => a.MerchantBranchId == model.MerchantBranchID && a.ToDistance != null && a.FromDistance != null);
                        if (old_distance != null && old_distance.Count() > 0)
                        {
                            foreach (var item in old_distance)
                            {
                                await _repository.Delete(item);
                            }
                        }
                        var distance = model.DeliveryPrice.Where(a => a.ToDistance != null && a.FromDistance != null).ToList();
                        if (distance != null && distance.Count() > 0)
                        {
                            foreach (var item in distance)
                            {
                                MerchantDeliveryPrice obj = new MerchantDeliveryPrice()
                                {
                                    MerchantBranchId = model.MerchantBranchID.Value,
                                    LocationId = null,
                                    FromDistance = item.FromDistance,
                                    ToDistance = item.ToDistance,
                                    Amount = item.Amount == null ? 0 : item.Amount.Value,
                                    IsDeleted = false,
                                    ModifiedDate = DateTime.UtcNow,
                                    CreationDate = DateTime.UtcNow,
                                };
                                await _repository.Add(obj);
                            }
                        }
                        break;
                    case 3://offer
                        branch.DeliveryPriceOffer = model.Amount == null ? 0 : model.Amount.Value;
                        break;
                    default:
                        break;
                }
                await _Brepository.Update(branch);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }


        [HttpGet, Route("GetDeliveryPrices")]
        public async Task<ActionResult<DriverCase>> GetDeliveryPrices([FromQuery] long? MerchantBranchID)
        {
            try
            {
                if (MerchantBranchID == null)
                    return NotFound();
                BranchCaseDTO model = new BranchCaseDTO();
                var branch = await _Brepository.GetByID(MerchantBranchID.Value);
                if (branch == null)
                    return NotFound();

                switch (branch.DeliveryStatus)
                {
                    case (int)DeliveryStatus.Offer:
                        model.DeliveryStatus = (int)DeliveryStatus.Offer;
                        model.DeliveryPriceOffer = branch.DeliveryPriceOffer;
                        break;
                    case (int)DeliveryStatus.Distance:
                        model.DeliveryStatus = (int)DeliveryStatus.Distance;
                        model.MerchantDeliveryPrice = await _repository.getByBranchID(MerchantBranchID.Value, (int)DeliveryStatus.Distance);
                        break;
                    case (int)DeliveryStatus.Location:
                        model.DeliveryStatus = (int)DeliveryStatus.Location;
                        model.MerchantDeliveryPrice = await _repository.getByBranchID(MerchantBranchID.Value, (int)DeliveryStatus.Location);
                        break;
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        private bool MerchantDeliveryPriceExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
