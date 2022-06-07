using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iDeliverService.Controllers
{
    [Route("api/Order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly IDriverOrderRepository _Dorepository;
        private readonly IDriverRepository _Drrepository;
        private readonly IDriverDetailsRepository _DDepository;
        private readonly IDriverSchaduleRepository _Srepository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public OrderController(IOrderRepository repository, IDriverOrderRepository Dorepository, IDriverRepository Drrepository, IDriverDetailsRepository DDepository,
            IDriverSchaduleRepository Srepository,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _repository = repository;
            _Dorepository = Dorepository;
            _Drrepository = Drrepository;
            _DDepository = DDepository;
            _Srepository = Srepository;
            _env = env;
        }

        // GET: api/Order
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var result = await _repository.GetAll();

            return Ok(result);
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(long id)
        {
            var Order = await _repository.GetByID(id);

            if (Order == null)
            {
                return NotFound();
            }
            return Ok(Order);
        }

        [HttpPost("AddOrder")]
        public async Task<ActionResult<Order>> AddOrder()//([FromBody] OrderDTO obj)
        {
            try
            {
                // bool isExistUsername = _Urepository.IsExists(w => w.Username.ToLower() == obj.username);
                // if (isExistUsername) return BadRequest("username is exist");

                // bool isExistNationalNumber = _repository.IsExists(w => w.NationalNumber == obj.nationalNumber);
                // if (isExistNationalNumber) return BadRequest("national number is exist");

                //return Ok(OrderID);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetCurrentOrders")]
        public async Task<ActionResult<OrderDTO>> GetCurrentOrders()
        {
            try
            {
                var CurrentOrders = await _repository.GetCurrentOrders();
                return Ok(CurrentOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNewOrders")]
        public async Task<ActionResult<OrderDTO>> GetNewOrders()
        {
            try
            {
                var NewOrders = await _repository.GetNewOrders();
                return Ok(NewOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AssignOrderToDriver")]
        public async Task<ActionResult<Order>> AssignOrderToDriver([FromBody] OrderDTO model)
        {
            try
            {
                if (model == null || model.Id==null || model.DriverID == null)
                    return NotFound();

                var order = await _repository.GetByID(model.Id.Value);
                if (order == null)
                {
                    return NotFound();
                }
                else
                {
                    DriverOrder driver = new DriverOrder()
                    {
                        Status = (int)DriverOrderEnum.PenddingOrder,
                        CreationDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        DriverId = model.DriverID.Value,
                        OrderId = model.Id.Value,
                        IsDeleted = false,
                        Note = "",
                    };
                    await _Dorepository.Add(driver);
                    order.Status = (int)OrderStatus.AssignToDriver;
                    await _repository.Update(order);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private bool OrderExists(long id)
        {
            return _repository.IsExists(w => w.Id == id);
        }


        [HttpPost, Route("DeleteOrder")]
        public async Task<ActionResult> DeleteOrder([FromBody] long OrderID)
        {

            try
            {
                Order Order = await _repository.GetByID(OrderID);
                if (Order == null) return BadRequest("Order Not Found");

                Order.IsDeleted = true;
                await _repository.Update(Order);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        //[HttpPost, Route("ActiveOrder")]
        //public async Task<ActionResult> ActiveOrder([FromBody] long OrderID)
        //{

        //    try
        //    {
        //        Order Order = await _repository.GetByID(OrderID);
        //        if (Order == null) return BadRequest("Order Not Found");

        //        Order.IsActive = !Order.IsActive;
        //        await _repository.Update(Order);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(ex.Message);
        //    }

        //}
    }
}
