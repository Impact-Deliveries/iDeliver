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
    internal class MerchantRepository : IMerchantRepository
    {

        private readonly IDeliverDbContext _context;

        public MerchantRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Merchant>> GetAll() =>
            await _context.Merchants.ToListAsync();

        public async Task<Merchant?> GetFirstRow() =>
            await _context.Merchants.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<Merchant?> GetLastRow() =>
            await _context.Merchants.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<Merchant?> GetByID(long id) =>
            await _context.Merchants.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Merchant>> Find(Expression<Func<Merchant, bool>> where) =>
            await _context.Merchants.Where(where).ToListAsync();

        public async Task<Merchant?> FindRow(Expression<Func<Merchant, bool>> where) =>
            await _context.Merchants.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<Merchant, bool>> where) =>
             _context.Merchants.Any(where);

        public async Task Add(Merchant Merchant)
        {
            _context.Merchants.Add(Merchant);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(Merchant Merchant)
        {
            _context.Entry(Merchant).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Merchant Merchant)
        {
            try
            {
                _context.Merchants.Remove(Merchant);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task<List<Merchant>> GetMerchants(string? MerchantName, bool? IsActive = true)
        {
            try
            {
                var data = _context.Merchants.Where(a =>
                (string.IsNullOrEmpty(MerchantName) ? 1 == 1 : a.MerchantName.Contains(MerchantName)) && 
               (IsActive==null ? 1 == 1 : a.IsActive == IsActive));
                return await data.ToListAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

    }
}