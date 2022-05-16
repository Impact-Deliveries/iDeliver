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
    internal class MerchantDeliveryPriceRepository : IMerchantDeliveryPriceRepository
    {

        private readonly IDeliverDbContext _context;

        public MerchantDeliveryPriceRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MerchantDeliveryPrice>> GetAll() =>
            await _context.MerchantDeliveryPrices.ToListAsync();

        public async Task<MerchantDeliveryPrice?> GetFirstRow() =>
            await _context.MerchantDeliveryPrices.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<MerchantDeliveryPrice?> GetLastRow() =>
            await _context.MerchantDeliveryPrices.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<MerchantDeliveryPrice?> GetByID(long id) =>
            await _context.MerchantDeliveryPrices.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<MerchantDeliveryPrice>> Find(Expression<Func<MerchantDeliveryPrice, bool>> where) =>
            await _context.MerchantDeliveryPrices.Where(where).ToListAsync();

        public async Task<MerchantDeliveryPrice?> FindRow(Expression<Func<MerchantDeliveryPrice, bool>> where) =>
            await _context.MerchantDeliveryPrices.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<MerchantDeliveryPrice, bool>> where) =>
             _context.MerchantDeliveryPrices.Any(where);

        public async Task Add(MerchantDeliveryPrice MerchantDeliveryPrice)
        {
            _context.MerchantDeliveryPrices.Add(MerchantDeliveryPrice);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(MerchantDeliveryPrice MerchantDeliveryPrice)
        {
            _context.Entry(MerchantDeliveryPrice).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(MerchantDeliveryPrice MerchantDeliveryPrice)
        {
            try
            {
                _context.MerchantDeliveryPrices.Remove(MerchantDeliveryPrice);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

    
    }
}