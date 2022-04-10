
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
    internal class AttachmentRepository : IAttachmentRepository
    {

        private readonly IDeliverDbContext _context;

        public AttachmentRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attachment>> GetAll() =>
            await _context.Attachments.ToListAsync();

        public async Task<Attachment?> GetFirstRow() =>
            await _context.Attachments.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<Attachment?> GetLastRow() =>
            await _context.Attachments.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<Attachment?> GetByID(long id) =>
            await _context.Attachments.Where(w => w.Id == id ).FirstOrDefaultAsync();

        public async Task<IEnumerable<Attachment>> Find(Expression<Func<Attachment, bool>> where) =>
            await _context.Attachments.Where(where).ToListAsync();

        public async Task<Attachment?> FindRow(Expression<Func<Attachment, bool>> where) =>
            await _context.Attachments.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<Attachment, bool>> where) =>
             _context.Attachments.Any(where);

        public async Task Add(Attachment Attachment)
        {
            _context.Attachments.Add(Attachment);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(Attachment Attachment)
        {
            _context.Entry(Attachment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(Attachment Attachment)
        {
            try
            {
                _context.Attachments.Remove(Attachment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}