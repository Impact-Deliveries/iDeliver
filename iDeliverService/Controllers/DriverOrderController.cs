using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/DriverOrder")]
    [ApiController]
    [Authorize]
    public class DriverOrderController : ControllerBase
    {
        private readonly IDriverOrderRepository _repository;

        public DriverOrderController(IDriverOrderRepository repository)
        {
            _repository = repository;
        }

        // GET: api/DriverOrder
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<DriverOrder>>> GetAllDriverOrders()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }


        // GET: api/DriverOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverOrder>> GetDriverOrder(long id)
        {
            var DriverOrder = await _repository.GetByID(id);

            if (DriverOrder == null)
            {
                return NotFound();
            }
            return Ok(DriverOrder);
        }

        //[HttpPost("AddDriverOrder")]
        //public async Task<ActionResult<DriverOrderDTO>> AddDriverOrder(DriverOrderDTO modal)
        //{
        //    try
        //    {
        //        if (modal.Id != null && modal.Id > 0)
        //        {
        //            var DriverOrder = await _repository.GetByID(modal.Id.Value);
        //            if (DriverOrder != null)
        //            {
        //                DriverOrder.ModifiedDate = DateTime.UtcNow;
        //                DriverOrder.Phone = modal.Phone;
        //                DriverOrder.Overview = modal.Overview;
        //                DriverOrder.DriverOrderName = modal.DriverOrderName;
        //                DriverOrder.Email = modal.Email;
        //                DriverOrder.Mobile = modal.Mobile;
        //                DriverOrder.IsActive = modal.IsActive;
        //                DriverOrder.Owner = modal.Owner;
        //                DriverOrder.OwnerNumber = modal.OwnerNumber;
        //                DriverOrder.Position = modal.Position;
        //                DriverOrder.QutationNumber = modal.QutationNumber;
        //                await _repository.Update(DriverOrder);
        //            }


        //        }
        //        else
        //        {
        //            DriverOrder DriverOrder = new DriverOrder()
        //            {
        //                ModifiedDate = DateTime.UtcNow,
        //                Phone = modal.Phone,
        //                Overview = modal.Overview,
        //                DriverOrderName = modal.DriverOrderName,
        //                Email = modal.Email,
        //                Mobile = modal.Mobile,
        //                IsActive = true,
        //                CreationDate = DateTime.UtcNow,
        //                OrganizationId = modal.OrganizationId != null ? modal.OrganizationId.Value : 1,
        //                Owner = modal.Owner,
        //                Position = modal.Position,
        //                QutationNumber = modal.QutationNumber,
        //                OwnerNumber = modal.OwnerNumber
        //        };
        //            await _repository.Add(DriverOrder);
        //        }
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest();
        //    }

        //}


        private bool DriverOrderExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }

    }
}
