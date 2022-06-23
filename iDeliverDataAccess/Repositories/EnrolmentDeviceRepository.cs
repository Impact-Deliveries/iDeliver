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
    internal class EnrolmentDeviceRepository : IEnrolmentDeviceRepository
    {
        private readonly IDeliverDbContext _context;

        public EnrolmentDeviceRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnrolmentDevice>> GetAll() =>
            await _context.EnrolmentDevices.ToListAsync();

        public async Task<EnrolmentDevice?> GetFirstRow() =>
            await _context.EnrolmentDevices.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<EnrolmentDevice?> GetLastRow() =>
            await _context.EnrolmentDevices.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<EnrolmentDevice?> GetByID(long id) =>
            await _context.EnrolmentDevices.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<EnrolmentDevice>> Find(Expression<Func<EnrolmentDevice, bool>> where) =>
            await _context.EnrolmentDevices.Where(where).ToListAsync();

        public async Task<EnrolmentDevice?> FindRow(Expression<Func<EnrolmentDevice, bool>> where) =>
            await _context.EnrolmentDevices.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<EnrolmentDevice, bool>> where) =>_context.EnrolmentDevices.Any(where);

        public async Task Add(EnrolmentDevice entity)
        {
            _context.EnrolmentDevices.Add(entity);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(EnrolmentDevice entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Delete(EnrolmentDevice entity)
        {
            try
            {
                _context.EnrolmentDevices.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}
