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

        public async Task<Order?> GetByID(long id) =>
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



    }
}