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
    internal class LocationRepository : ILocationRepository
    {

        private readonly IDeliverDbContext _context;

        public LocationRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetAll() =>
            await _context.Locations.ToListAsync();

        public async Task<Location?> GetFirstRow() =>
            await _context.Locations.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<Location?> GetLastRow() =>
            await _context.Locations.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<Location?> GetByID(long id) =>
            await _context.Locations.Where(w => w.Id == id ).FirstOrDefaultAsync();

        public async Task<IEnumerable<Location>> Find(Expression<Func<Location, bool>> where) =>
            await _context.Locations.Where(where).ToListAsync();

        public async Task<Location?> FindRow(Expression<Func<Location, bool>> where) =>
            await _context.Locations.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<Location, bool>> where) =>
             _context.Locations.Any(where);

        public async Task Add(Location Location)
        {
            _context.Locations.Add(Location);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(Location Location)
        {
            _context.Entry(Location).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Location Location)
        {
            try
            {
                _context.Locations.Remove(Location);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

    }
}