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
    internal class DriverOrderRepository : IDriverOrderRepository
    {

        private readonly IDeliverDbContext _context;

        public DriverOrderRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DriverOrder>> GetAll() =>
            await _context.DriverOrders.ToListAsync();

        public async Task<DriverOrder?> GetFirstRow() =>
            await _context.DriverOrders.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<DriverOrder?> GetLastRow() =>
            await _context.DriverOrders.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<DriverOrder?> GetByID(long id) =>
            await _context.DriverOrders.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<DriverOrder>> Find(Expression<Func<DriverOrder, bool>> where) =>
            await _context.DriverOrders.Where(where).ToListAsync();

        public async Task<IEnumerable<DriverOrder>> GetDriverOrders(Expression<Func<DriverOrder, bool>> where) =>
            await _context.DriverOrders.Include("Order")
                        .Include("Order.MerchantBranch")
                        .Include("Order.MerchantBranch.Merchant")
            .Where(where).ToListAsync();

        public async Task<DriverOrder?> FindRow(Expression<Func<DriverOrder, bool>> where) =>
            await _context.DriverOrders.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<DriverOrder, bool>> where) =>
             _context.DriverOrders.Any(where);

        public async Task Add(DriverOrder DriverOrder)
        {
            _context.DriverOrders.Add(DriverOrder);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(DriverOrder DriverOrder)
        {
            _context.Entry(DriverOrder).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(DriverOrder DriverOrder)
        {
            try
            {
                _context.DriverOrders.Remove(DriverOrder);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<DriverOrder?> GetHoldDriverOrder(long driverID)
        {

            List<int> iOrderStatus = new List<int>() { (int)OrderStatus.DriverRejected, (int)OrderStatus.OrderCompleted };

            return await _context.DriverOrders
                        .Include("Order")
                        .Include("Order.MerchantBranch")
                        .Include("Order.MerchantBranch.Merchant")
                        .Where(w => w.DriverId == driverID && w.IsDeleted == false && !iOrderStatus.Contains(w.Order.Status))
                        .OrderByDescending(o => o.ModifiedDate)
                        .FirstOrDefaultAsync();
        }

        public async Task<DriverOrder?> GetOrder(long orderID, long driverID)
        {
            List<int> iOrderStatus = new List<int>() { (int)OrderStatus.DriverRejected, (int)OrderStatus.OrderCompleted };
            return await _context.DriverOrders
                        .Include("Order")
                        .Include("Order.MerchantBranch")
                        .Include("Order.MerchantBranch.Merchant")
                        .Where(w => w.DriverId == driverID && w.OrderId == orderID && w.IsDeleted == false && !iOrderStatus.Contains(w.Order.Status))
                        .OrderByDescending(o => o.ModifiedDate)
                        .FirstOrDefaultAsync();
        }

    }
}