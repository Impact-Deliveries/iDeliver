using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using iDeliverDataAccess.Base;
using IDeliverObjects.Objects;
using Microsoft.EntityFrameworkCore;

namespace iDeliverDataAccess.Repositories
{
    internal class DriverDetailRepository : IDriverDetailsRepository
    {

        private readonly IDeliverDbContext _context;

        public DriverDetailRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DriverDetail>> GetAll() =>
            await _context.DriverDetails.ToListAsync();

        public async Task<DriverDetail?> GetFirstRow() =>
            await _context.DriverDetails.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<DriverDetail?> GetLastRow() =>
            await _context.DriverDetails.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<DriverDetail?> GetByID(long id) =>
            await _context.DriverDetails.Where(w => w.Id == id ).FirstOrDefaultAsync();

        public async Task<IEnumerable<DriverDetail>> Find(Expression<Func<DriverDetail, bool>> where) =>
            await _context.DriverDetails.Where(where).ToListAsync();

        public async Task<DriverDetail?> FindRow(Expression<Func<DriverDetail, bool>> where) =>
            await _context.DriverDetails.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<DriverDetail, bool>> where) =>
             _context.DriverDetails.Any(where);

        public async Task Add(DriverDetail DriverDetail)
        {
            _context.DriverDetails.Add(DriverDetail);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(DriverDetail DriverDetail)
        {
            _context.Entry(DriverDetail).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(DriverDetail DriverDetail)
        {
            try
            {
                _context.DriverDetails.Remove(DriverDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}