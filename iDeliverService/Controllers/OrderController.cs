using iDeliverDataAccess.Repositories;
using IDeliverObjects.DTO;
using IDeliverObjects.DTO.Notification.Models;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;

using iDeliverService.Common.Service;
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
        private readonly IDriverOrderRepository _driverOrderRepository;
        private readonly IEnrolmentDeviceRepository _enrolmentDeviceRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IDriverCaseRepository _driverCaseRepository;
        private readonly IMerchantBranchRepository _merchantBranchRepository;
        private readonly INotificationService<OrderNotification> _notificationService;

        public OrderController(IOrderRepository repository,
            IDriverOrderRepository driverOrderRepository,
            IEnrolmentDeviceRepository enrolmentDeviceRepository,
            IDriverRepository driverRepository,
            IDriverCaseRepository driverCaseRepository,
            IMerchantBranchRepository merchantBranchRepository,
            INotificationService<OrderNotification> notificationService)
        {
            _repository = repository;
            _driverOrderRepository = driverOrderRepository;
            _enrolmentDeviceRepository = enrolmentDeviceRepository;
            _driverRepository = driverRepository;
            _driverCaseRepository = driverCaseRepository;
            _merchantBranchRepository = merchantBranchRepository;
            _notificationService = notificationService;
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
        public async Task<ActionResult> AssignOrderToDriver([FromBody] OrderDTO model)
        {
            try
            {
                if (model is null || model.Id is null || model.DriverID is null)
                    return NotFound();

                var order = await _repository.GetByID(model.Id.Value);
                if (order is null)
                {
                    return NotFound();
                }
                else
                {

                    // assgin order to driver
                    DriverOrder driverOrder = new DriverOrder()
                    {
                        Status = (int)DriverOrderEnum.PendingOrder,
                        CreationDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        DriverId = model.DriverID.Value,
                        OrderId = model.Id.Value,
                        IsDeleted = false,
                        Note = "",
                    };

                    await _driverOrderRepository.Add(driverOrder);

                    order.Status = (int)OrderStatus.AssignToDriver;
                    await _repository.Update(order);


                    // Send Notification
                    var driver = await _driverRepository.FindRow(f => f.Id == model.DriverID.Value);

                    if (driver is not null)
                    {
                        var enrolmentDevice = await _enrolmentDeviceRepository.FindRow(f => f.EnrolmentId == driver.EnrolmentId &&
                        f.IsDeleted == false);

                        if (enrolmentDevice is not null)
                        {
                            var merchantBranch = await _merchantBranchRepository.FindRow(f => f.Id == order.MerchantBranchId && f.IsActive == true);

                            if (merchantBranch is not null)
                            {
                                IDeliverObjects.DTO.Notification.Notification<OrderNotification>
                                    notification = new IDeliverObjects.DTO.Notification.Notification<OrderNotification>()
                                    {
                                        DeviceId = enrolmentDevice.DeviceToken,
                                        Title = $"Order #{order.Id}",
                                        Body = $"Order from {order.MerchantBranch}",
                                        IsAndroiodDevice = true,
                                        Data = new OrderNotification()
                                        {
                                            OrderID = order.Id,
                                            Note = order.Note,
                                            Status = order.Status,
                                            ClientName = order.ClientName,
                                            ClientNumber = order.ClientNumber,
                                            DriverID = driver.Id,
                                            DriverOrderNote = driverOrder.Note,
                                            DriverOrderStatus = driverOrder.Status,
                                            MerchantBranchID = order.MerchantBranchId,
                                            MerchantName = merchantBranch.Merchant.MerchantName
                                        }
                                    };

                                var result = await _notificationService.SendNotification(notification);
                            }
                        }
                    }

                    return Ok();
                }

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


        [HttpPost("AddOrder")]
        public async Task<ActionResult> AddOrder([FromBody] OrderDTO model)
        {
            try
            {
                if (model == null)
                    return BadRequest();

                Order order = new Order()
                {
                    CreationDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    IsDeleted = false,
                    Note = model.Note,
                    Status = (model.Status == null || model.Status == 0) ? (short)OrderStatus.PendingOrder : model.Status.Value,
                    TotalAmount = model.TotalAmount != null ? model.TotalAmount.Value : 0,
                    DeliveryAmount = model.DeliveryAmount != null ? model.DeliveryAmount.Value : 0,
                    MerchantBranchId = model.MerchantBranchId.Value,
                    ClientNumber = model.ClientNumber,
                    ClientName = model.ClientName,
                    MerchantDeliveryPriceId = model.MerchantDeliveryPriceID == null || model.MerchantDeliveryPriceID == 0 ? null : model.MerchantDeliveryPriceID
                };


                await _repository.Add(order);

                if (order.Status == 2)
                {
                    #region assign order to driver
                    DriverOrder driverOrder = new DriverOrder()
                    {
                        Status = (int)DriverOrderEnum.PendingOrder,
                        CreationDate = DateTime.UtcNow,
                        ModifiedDate = DateTime.UtcNow,
                        DriverId = model.DriverID.Value,
                        OrderId = order.Id,
                        IsDeleted = false,
                        Note = "",
                    };
                    await _driverOrderRepository.Add(driverOrder);

                    order.Status = (int)OrderStatus.AssignToDriver;
                    await _repository.Update(order);
                    #endregion

                    // Send Notification
                    var driver = await _driverRepository.FindRow(f => f.Id == model.DriverID.Value);

                    if (driver is not null)
                    {
                        var enrolmentDevice = await _enrolmentDeviceRepository.FindRow(f => f.EnrolmentId == driver.EnrolmentId &&
                        f.IsDeleted == false);

                        if (enrolmentDevice is not null)
                        {
                            var merchantBranch = await _merchantBranchRepository.FindRow(f => f.Id == order.MerchantBranchId && f.IsActive == true);

                            if (merchantBranch is not null)
                            {
                                IDeliverObjects.DTO.Notification.Notification<OrderNotification>
                                    notification = new IDeliverObjects.DTO.Notification.Notification<OrderNotification>()
                                    {
                                        DeviceId = enrolmentDevice.DeviceToken,
                                        Title = $"Order #{order.Id}",
                                        Body = $"Order from {merchantBranch.Merchant.MerchantName}",
                                        IsAndroiodDevice = true,
                                        Module = (int)IDeliverObjects.Enum.NotificationModule.DriverOrder,
                                        Data = new OrderNotification()
                                        {
                                            OrderID = order.Id,
                                            Note = order.Note,
                                            Status = order.Status,
                                            ClientName = order.ClientName,
                                            ClientNumber = order.ClientNumber,
                                            DriverID = driver.Id,
                                            DriverOrderNote = driverOrder.Note,
                                            DriverOrderStatus = driverOrder.Status,
                                            MerchantBranchID = order.MerchantBranchId,
                                            MerchantName = merchantBranch.Merchant.MerchantName
                                        }
                                    };

                                var result = await _notificationService.SendNotification(notification);
                            }
                        }
                    }


                }

                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

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

        [HttpPost("GetOrders")]
        public async Task<IActionResult> GetOrders([FromQuery] NgTableParam<NgOrderTable> request, [FromBody] NgOrderTable objects)
        {
            try
            {
                NgTableResult<OrderDTO> results = new NgTableResult<OrderDTO>();
                var page_index = request.page == 0 ? request.page : request.page - 1;
                var page_skips = page_index * request.count;
                int total = 0;

                var orders = await _repository.GetOrders(objects);

                results = new NgTableResult<OrderDTO>()
                {
                    results = orders.Distinct()
                    .OrderByDescending(o => o.OrderDate)
                    .Skip(page_skips).Take(request.count).ToList(),
                    total = orders.Count()
                };
                return Ok(results);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("SetOrderStatus")]
        public async Task<ActionResult> SetOrderStatus([FromBody] OrderDTO model)
        {
            try
            {
                /*
                 * model.id => orderID
                 * model.DriverID => driverID
                 * model.Status => Enum.OrderStatus
                 * model.EnrolmentID => enrolmentID
                 */
                var driverOrder = await _driverOrderRepository.FindRow(f => f.DriverId == model.DriverID &&
                f.IsDeleted == false && f.Status !=
                (short)IDeliverObjects.Enum.DriverOrderEnum.RejectedOrder && f.OrderId == model.Id);

                if (driverOrder is not null)
                {
                    short driverCaseStatus = (short)IDeliverObjects.Enum.DriverCaseStatus.hold;
                    switch (model.Status.Value)
                    {
                        case (int)IDeliverObjects.Enum.OrderStatus.DriverAccepted:
                            driverOrder.Status = (int)IDeliverObjects.Enum.DriverOrderEnum.AcceptedOrder;
                            driverCaseStatus = (short)IDeliverObjects.Enum.DriverCaseStatus.hold;
                            break;
                        case (int)IDeliverObjects.Enum.OrderStatus.DriverRejected:
                            driverCaseStatus = (short)(short)IDeliverObjects.Enum.DriverCaseStatus.available;
                            driverOrder.Status = (int)IDeliverObjects.Enum.DriverOrderEnum.RejectedOrder;
                            break;
                        case (int)IDeliverObjects.Enum.OrderStatus.PendingOrder:
                            driverCaseStatus = (short)IDeliverObjects.Enum.DriverCaseStatus.available;
                            driverOrder.Status = (int)IDeliverObjects.Enum.DriverOrderEnum.PendingOrder;
                            break;
                        case (int)IDeliverObjects.Enum.OrderStatus.OrderCompleted:
                            driverCaseStatus = (short)IDeliverObjects.Enum.DriverCaseStatus.available;
                            break;
                        default:
                            driverCaseStatus = (short)IDeliverObjects.Enum.DriverCaseStatus.hold;
                            break;

                    }

                    driverOrder.ModifiedDate = DateTime.UtcNow;

                    var order = await _repository.FindRow(f => f.Id == driverOrder.Id && f.IsDeleted == false);

                    if (order is not null)
                    {
                        order.Status = model.Status.Value;
                        order.ModifiedDate = DateTime.UtcNow;

                        await _repository.Update(order).ConfigureAwait(false);
                        await _driverOrderRepository.Update(driverOrder).ConfigureAwait(false);
                        await _driverCaseRepository.SetDriverCase(new DriverCaseDTO()
                        {
                            EnrolmentID = model.EnrolmentID.Value,
                            Status = driverCaseStatus,
                        }).ConfigureAwait(false);
                        
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
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
