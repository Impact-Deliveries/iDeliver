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
    internal class OrganizationRepository : IOrganizationRepository
    {

        private readonly IDeliverDbContext _context;

        public OrganizationRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Organization>> GetAll() =>
            await _context.Organizations.ToListAsync();

        public async Task<Organization?> GetFirstRow() =>
            await _context.Organizations.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<Organization?> GetLastRow() =>
            await _context.Organizations.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<Organization?> GetByID(long id) =>
            await _context.Organizations.Where(w => w.Id == id ).FirstOrDefaultAsync();

        public async Task<IEnumerable<Organization>> Find(Expression<Func<Organization, bool>> where) =>
            await _context.Organizations.Where(where).ToListAsync();

        public async Task<Organization?> FindRow(Expression<Func<Organization, bool>> where) =>
            await _context.Organizations.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<Organization, bool>> where) =>
             _context.Organizations.Any(where);

        public async Task Add(Organization Organization)
        {
            _context.Organizations.Add(Organization);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(Organization Organization)
        {
            _context.Entry(Organization).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Organization Organization)
        {
            try
            {
                _context.Organizations.Remove(Organization);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

    }
}