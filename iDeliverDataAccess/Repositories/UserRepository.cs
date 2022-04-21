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
    internal class UserRepository : IUserRepository
    {

        private readonly IDeliverDbContext _context;

        public UserRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll() =>
            await _context.Users.ToListAsync();

        public async Task<User?> GetFirstRow() =>
            await _context.Users.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<User?> GetLastRow() =>
            await _context.Users.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<User?> GetByID(long id) =>
            await _context.Users.Where(w => w.Id == id && w.IsActive == true).FirstOrDefaultAsync();

        public async Task<IEnumerable<User>> Find(Expression<Func<User, bool>> where) =>
            await _context.Users.Where(where).ToListAsync();

        public async Task<User?> FindRow(Expression<Func<User, bool>> where) =>
            await _context.Users.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<User, bool>> where) =>
             _context.Users.Any(where);

        public async Task Add(User user)
        {
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(User user)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task<int> GetuserCount() {
            try
            {
                return _context.Users.Count();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

    }
}