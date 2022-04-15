

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
    internal class DriverSchaduleRepository : IDriverSchaduleRepository
    {

        private readonly IDeliverDbContext _context;

        public DriverSchaduleRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DriverSchadule>> GetAll() =>
            await _context.DriverSchadules.ToListAsync();

        public async Task<DriverSchadule?> GetFirstRow() =>
            await _context.DriverSchadules.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<DriverSchadule?> GetLastRow() =>
            await _context.DriverSchadules.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<DriverSchadule?> GetByID(long id) =>
            await _context.DriverSchadules.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<DriverSchadule>> Find(Expression<Func<DriverSchadule, bool>> where) =>
            await _context.DriverSchadules.Where(where).ToListAsync();

        public async Task<DriverSchadule?> FindRow(Expression<Func<DriverSchadule, bool>> where) =>
            await _context.DriverSchadules.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<DriverSchadule, bool>> where) =>
             _context.DriverSchadules.Any(where);

        public async Task Add(DriverSchadule DriverSchadule)
        {
            _context.DriverSchadules.Add(DriverSchadule);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(DriverSchadule DriverSchadule)
        {
            _context.Entry(DriverSchadule).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(DriverSchadule DriverSchadule)
        {
            try
            {
                _context.DriverSchadules.Remove(DriverSchadule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<List<DriverSchadule>> GetByDriverID(long id)
        {
            try
            {
                return await _context.DriverSchadules.Where(a => a.DriverId == id).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteScheduleByDriverID(List<DriverSchadule> selected)
        {
             _context.DriverSchadules.RemoveRange(selected);
            await _context.SaveChangesAsync();
        }

    }
}