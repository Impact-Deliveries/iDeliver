using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using iDeliverDataAccess.Base;
using IDeliverObjects.DTO;
using IDeliverObjects.Enum;
using IDeliverObjects.Objects;
using Microsoft.EntityFrameworkCore;

namespace iDeliverDataAccess.Repositories
{
    internal class OrderRepository : IOrderRepository
    {

        private readonly IDeliverDbContext _context;

        public OrderRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAll() =>
            await _context.Orders.ToListAsync();

        public async Task<Order?> GetFirstRow() =>
            await _context.Orders.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<Order?> GetLastRow() =>
            await _context.Orders.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<Order> GetByID(long id) =>
            await _context.Orders.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Order>> Find(Expression<Func<Order, bool>> where) =>
            await _context.Orders.Where(where).ToListAsync();

        public async Task<Order?> FindRow(Expression<Func<Order, bool>> where) =>
            await _context.Orders.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<Order, bool>> where) =>
             _context.Orders.Any(where);

        public async Task Add(Order Order)
        {
            _context.Orders.Add(Order);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(Order Order)
        {
            _context.Entry(Order).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Order Order)
        {
            try
            {
                _context.Orders.Remove(Order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<List<OrderDTO?>> GetCurrentOrders()
        {
            try
            {
                var data = await (from order in _context.Orders
                                  join branch in _context.MerchantBranches on order.MerchantBranchId equals branch.Id
                                  join merchant in _context.Merchants on branch.MerchantId equals merchant.Id
                                  join DriverOrders in _context.DriverOrders on order.Id equals DriverOrders.DriverId
                                  join driver in _context.Drivers on DriverOrders.DriverId equals driver.Id
                                  where order.IsDeleted == false && branch.IsActive == true && merchant.IsActive == true
                                   && (order.Status != 1 && order.Status != 2 && order.Status != 3)
                                  select new OrderDTO
                                  {
                                      Id = order.Id,
                                      MerchantBranchId = order.MerchantBranchId,
                                      TotalAmount = order.TotalAmount,
                                      DeliveryAmount = order.DeliveryAmount,
                                      MerchantName = merchant.MerchantName + "-" + branch.BranchName,
                                      MerchantPhone = branch.Phone,
                                      Status = order.Status,
                                      ClientName = order.ClientName,
                                      ClientNumber = order.ClientNumber,
                                      Note = order.Note,
                                      OrderDate = order.CreationDate,
                                      DriverName = driver.FirstName + " " + driver.LastName,
                                      DriverPhone = driver.Phone,
                                  }).Distinct().OrderBy(a => a.OrderDate).ToListAsync();
                return data;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<List<OrderDTO?>> GetNewOrders()
        {
            try
            {
                var data = (from order in _context.Orders
                            join branch in _context.MerchantBranches on order.MerchantBranchId equals branch.Id
                            join merchant in _context.Merchants on branch.MerchantId equals merchant.Id
                            // join merchantDeliveryPrices in _context.MerchantDeliveryPrices on order.MerchantDeliveryPriceId equals merchantDeliveryPrices.Id
                            where order.IsDeleted == false && branch.IsActive == true && merchant.IsActive == true
                            && (order.Status == 1 || order.Status == 2 || order.Status == 3)
                            select new OrderDTO
                            {
                                Id = order.Id,
                                MerchantBranchId = order.MerchantBranchId,
                                TotalAmount = order.TotalAmount,
                                DeliveryAmount = order.DeliveryAmount,
                                MerchantName = merchant.MerchantName + "-" + branch.BranchName,
                                MerchantPhone = branch.Phone,
                                Status = order.Status,
                                Note = order.Note,
                                OrderDate = order.CreationDate,
                                ClientName = order.ClientName,
                                ClientNumber = order.ClientNumber,
                                DriverID = order.Status == 2 || order.Status == 3 ? (from c in _context.DriverOrders where order.Id == c.OrderId && (c.Status == 1 || c.Status == 3) select c.DriverId).FirstOrDefault() : 0,
                                LocationID = 0,
                                LocationName = (branch.DeliveryStatus == 1 ?
                                (from c in _context.MerchantDeliveryPrices
                                 join d in _context.Locations on c.LocationId equals d.Id
                                 where order.MerchantDeliveryPriceId == c.Id
                                 select d.Address).FirstOrDefault() : (branch.DeliveryStatus == 2 ?
                                 (from c in _context.MerchantDeliveryPrices
                                  where order.MerchantDeliveryPriceId == c.Id
                                  select c.FromDistance.ToString() + "-" + c.ToDistance.ToString()).FirstOrDefault()
                                 : ""))
                                //merchantDeliveryPrices.LocationId != null ? (from c in _context.Locations where c.Id == merchantDeliveryPrices.LocationId select c.Address).FirstOrDefault() : "",
                            });
                return await data.Distinct().OrderBy(a => a.OrderDate).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<OrderDTO>> GetOrders(NgOrderTable model)
        {
            try
            {

                if (model.driverID != null && model.driverID > 0)
                {
                    var data = (from order in _context.Orders
                                join driverorder in _context.DriverOrders on order.Id equals driverorder.OrderId
                                join branch in _context.MerchantBranches on order.MerchantBranchId equals branch.Id
                                join merchant in _context.Merchants on branch.MerchantId equals merchant.Id
                                // join merchantDeliveryPrices in _context.MerchantDeliveryPrices on order.MerchantDeliveryPriceId equals merchantDeliveryPrices.Id
                                where order.Status > 1
                               && order.IsDeleted == false && branch.IsActive == true && merchant.IsActive == true
                                && (model.fromdate != null ? order.CreationDate.Date >= model.fromdate.Value : 1 == 1)
                                && (model.toDate != null ? order.CreationDate.Date >= model.toDate.Value : 1 == 1)
                                && (model.merchantBranchID != null && model.merchantBranchID > 0 ? order.MerchantBranchId == model.merchantBranchID : 1 == 1)
                                && (model.status != null && model.status > 0 ? order.Status == model.status : 1 == 1)
                                && (model.driverID != null && model.driverID > 0 ? driverorder.DriverId == model.driverID : 1 == 1)

                                select new OrderDTO
                                {
                                    Id = order.Id,
                                    MerchantBranchId = order.MerchantBranchId,
                                    TotalAmount = order.TotalAmount,
                                    DeliveryAmount = order.DeliveryAmount,
                                    MerchantName = merchant.MerchantName + "-" + branch.BranchName,
                                    MerchantPhone = branch.Phone,
                                    Status = order.Status,
                                    Note = order.Note,
                                    OrderDate = order.CreationDate,
                                    ClientName = order.ClientName,
                                    ClientNumber = order.ClientNumber,
                                    DriverName = order.Status > 1 ? (from c2 in _context.Drivers
                                                                     where order.Id == driverorder.OrderId && driverorder.DriverId == c2.Id && (driverorder.Status == 2)
                                                                     select c2.FirstName + ' ' + c2.LastName).FirstOrDefault() : "",
                                    LocationID = 0,
                                    LocationName = (branch.DeliveryStatus == 1 ?
                                    (from c in _context.MerchantDeliveryPrices
                                     join d in _context.Locations on c.LocationId equals d.Id
                                     where order.MerchantDeliveryPriceId == c.Id
                                     select d.Address).FirstOrDefault() : (branch.DeliveryStatus == 2 ?
                                     (from c in _context.MerchantDeliveryPrices
                                      where order.MerchantDeliveryPriceId == c.Id
                                      select c.FromDistance.ToString() + "-" + c.ToDistance.ToString()).FirstOrDefault()
                                     : ""))
                                    //merchantDeliveryPrices.LocationId != null ? (from c in _context.Locations where c.Id == merchantDeliveryPrices.LocationId select c.Address).FirstOrDefault() : "",
                                });
                    return await data.Distinct().OrderBy(a => a.OrderDate).ToListAsync();
                }
                else
                {
                    var data = (from order in _context.Orders
                                join branch in _context.MerchantBranches on order.MerchantBranchId equals branch.Id
                                join merchant in _context.Merchants on branch.MerchantId equals merchant.Id
                                // join merchantDeliveryPrices in _context.MerchantDeliveryPrices on order.MerchantDeliveryPriceId equals merchantDeliveryPrices.Id
                                where order.IsDeleted == false && branch.IsActive == true && merchant.IsActive == true
                                // && (model.fromdate != null ? order.CreationDate.Date >= model.fromdate.Value : 1 == 1)
                                // && (model.toDate != null ? order.CreationDate.Date >= model.toDate.Value : 1 == 1)
                                && (model.merchantBranchID != null && model.merchantBranchID > 0 ? order.MerchantBranchId == model.merchantBranchID : 1 == 1)
                                && (model.status != null && model.status > 0 ? order.Status == model.status : 1 == 1)
                                select new OrderDTO
                                {
                                    Id = order.Id,
                                    MerchantBranchId = order.MerchantBranchId,
                                    TotalAmount = order.TotalAmount,
                                    DeliveryAmount = order.DeliveryAmount,
                                    MerchantName = merchant.MerchantName + "-" + branch.BranchName,
                                    MerchantPhone = branch.Phone,
                                    Status = order.Status,
                                    Note = order.Note,
                                    OrderDate = order.CreationDate,
                                    ClientName = order.ClientName,
                                    ClientNumber = order.ClientNumber,
                                    DriverName = order.Status > 1 ? (from c in _context.DriverOrders
                                                                     join c2 in _context.Drivers on c.DriverId equals c2.Id
                                                                     where order.Id == c.OrderId && (c.Status == 2 || c.Status == 1)
                                                                     select c2.FirstName + ' ' + c2.LastName).FirstOrDefault() : "",
                                    LocationID = 0,
                                    LocationName = (branch.DeliveryStatus == 1 ?
                                    (from c in _context.MerchantDeliveryPrices
                                     join d in _context.Locations on c.LocationId equals d.Id
                                     where order.MerchantDeliveryPriceId == c.Id
                                     select d.Address).FirstOrDefault() : (branch.DeliveryStatus == 2 ?
                                     (from c in _context.MerchantDeliveryPrices
                                      where order.MerchantDeliveryPriceId == c.Id
                                      select c.FromDistance.ToString() + "-" + c.ToDistance.ToString()).FirstOrDefault()
                                     : ""))
                                    //merchantDeliveryPrices.LocationId != null ? (from c in _context.Locations where c.Id == merchantDeliveryPrices.LocationId select c.Address).FirstOrDefault() : "",
                                });
                    return await data.Distinct().OrderBy(a => a.OrderDate).ToListAsync();

                }


            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}