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
    internal class EnrolmentRepository : IEnrolmentRepository
    {

        private readonly IDeliverDbContext _context;

        public EnrolmentRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrolment>> GetAll() =>
            await _context.Enrolments.ToListAsync();

        public async Task<Enrolment?> GetFirstRow() =>
            await _context.Enrolments.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<Enrolment?> GetLastRow() =>
            await _context.Enrolments.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<Enrolment?> GetByID(long id) =>
            await _context.Enrolments.Where(w => w.Id == id ).FirstOrDefaultAsync();

        public async Task<IEnumerable<Enrolment>> Find(Expression<Func<Enrolment, bool>> where) =>
            await _context.Enrolments.Where(where).ToListAsync();

        public async Task<Enrolment?> FindRow(Expression<Func<Enrolment, bool>> where) =>
            await _context.Enrolments.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<Enrolment, bool>> where) =>
             _context.Enrolments.Any(where);

        public async Task Add(Enrolment Enrolment)
        {
            _context.Enrolments.Add(Enrolment);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(Enrolment Enrolment)
        {
            _context.Entry(Enrolment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Enrolment Enrolment)
        {
            try
            {
                _context.Enrolments.Remove(Enrolment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}