using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using iDeliverDataAccess.Base;
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using Microsoft.EntityFrameworkCore;

namespace iDeliverDataAccess.Repositories
{
    internal class DriverRepository : IDriverRepository
    {

        private readonly IDeliverDbContext _context;

        public DriverRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Driver>> GetAll() =>
            await _context.Drivers.ToListAsync();

        public async Task<Driver?> GetFirstRow() =>
            await _context.Drivers.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<Driver?> GetLastRow() =>
            await _context.Drivers.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<Driver?> GetByID(long id) =>
            await _context.Drivers.Where(w => w.Id == id && w.IsActive == true).FirstOrDefaultAsync();

        public async Task<IEnumerable<Driver>> Find(Expression<Func<Driver, bool>> where) =>
            await _context.Drivers.Where(where).ToListAsync();

        public async Task<Driver?> FindRow(Expression<Func<Driver, bool>> where) =>
            await _context.Drivers.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<Driver, bool>> where) =>
             _context.Drivers.Any(where);

        public async Task Add(Driver Driver)
        {
            _context.Drivers.Add(Driver);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(Driver Driver)
        {
            _context.Entry(Driver).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Driver Driver)
        {
            try
            {
                _context.Drivers.Remove(Driver);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<DriverTableDTO> GetDrivers(NgDriverTableFilter filter, int pageIndex, int pageSize)
        {
            try
            {
                DriverTableDTO data = new DriverTableDTO();
                bool isactive = filter!=null ?(filter.IsActive == 0 ? false : true):false;
                List<Driver> drivers = await (from a in _context.Drivers
                                              where (String.IsNullOrEmpty(filter.DriverName) ? 1 == 1 :
                       (a.FirstName.Contains(filter.DriverName)
                        || a.SecondName.Contains(filter.DriverName)
                        || a.LastName.Contains(filter.DriverName)))
                        && (filter.IsActive >= 0 ? a.IsActive == isactive : 1 == 1)
                                              select a)
                        .OrderBy(p => p.FirstName).ThenBy(a => a.SecondName).ThenBy(a => a.LastName)
                        .Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
                data.Total = drivers.Count;
                data.Drivers = drivers;

                return data;
            }
            catch (DbUpdateException)
            {
                throw;
            }

        }
    }
}