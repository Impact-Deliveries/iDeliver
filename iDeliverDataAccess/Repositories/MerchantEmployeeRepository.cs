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
    internal class MerchantEmployeeRepository : IMerchantEmployeeRepository
    {

        private readonly IDeliverDbContext _context;

        public MerchantEmployeeRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MerchantEmployee>> GetAll() =>
            await _context.MerchantEmployees.ToListAsync();

        public async Task<MerchantEmployee?> GetFirstRow() =>
            await _context.MerchantEmployees.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<MerchantEmployee?> GetLastRow() =>
            await _context.MerchantEmployees.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<MerchantEmployee?> GetByID(long id) =>
            await _context.MerchantEmployees.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<MerchantEmployee>> Find(Expression<Func<MerchantEmployee, bool>> where) =>
            await _context.MerchantEmployees.Where(where).ToListAsync();

        public async Task<MerchantEmployee?> FindRow(Expression<Func<MerchantEmployee, bool>> where) =>
            await _context.MerchantEmployees.Where(where).FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<MerchantEmployee, bool>> where) =>
             _context.MerchantEmployees.Any(where);

        public async Task Add(MerchantEmployee MerchantEmployee)
        {
            _context.MerchantEmployees.Add(MerchantEmployee);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(MerchantEmployee MerchantEmployee)
        {
            _context.Entry(MerchantEmployee).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(MerchantEmployee MerchantEmployee)
        {
            try
            {
                _context.MerchantEmployees.Remove(MerchantEmployee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task<MerchantEmployeeDTO> GetMerchantEmployeeById(long id) {

            try
            {
                int role = (int)Module.MerchantEmployee;
                var data = (from c in _context.MerchantEmployees
                            join b in _context.MerchantBranches on c.MerchantBranchId equals b.Id
                            where c.MerchantBranchId == id
                            select new MerchantEmployeeDTO
                            {
                                Id = b.Id,
                                EnrolmentId = c.EnrolmentId,
                                MerchantBranchId = c.MerchantBranchId,
                                NationalNumber = c.NationalNumber,
                                ProfilePicture = c.ProfilePicture,
                                FirstName = c.FirstName,
                                MiddleName = c.MiddleName,
                                LastName = c.LastName,
                                Mobile = c.Mobile,
                                MerchantBranchName = b.BranchName,
                                Phone = c.Phone,
                                IsActive = c.IsActive,
                                ModifiedDate = c.ModifiedDate,
                                CreationDate = c.CreationDate,
                                Attachments = (from d in _context.Attachments where id == d.ModuleId && d.ModuleType == role select d).ToList(),
                            }).FirstOrDefaultAsync();
                return await data;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<List<MerchantEmployeeDTO>> GetMerchantEmployeeByBranchId(long id, bool IsActive)
        {

            try
            {
                int role = (int)Module.MerchantEmployee;
                var data = (from c in _context.MerchantEmployees
                            join b in _context.MerchantBranches on c.MerchantBranchId equals b.Id
                            where c.MerchantBranchId == id && c.IsActive == IsActive    
                            select new MerchantEmployeeDTO
                            {
                                Id = b.Id,
                                EnrolmentId = c.EnrolmentId,
                                MerchantBranchId = c.MerchantBranchId,
                                NationalNumber = c.NationalNumber,
                                ProfilePicture = c.ProfilePicture,
                                FirstName = c.FirstName,
                                MiddleName = c.MiddleName,
                                LastName = c.LastName,
                                Mobile = c.Mobile,
                                Phone = c.Phone,
                                MerchantBranchName=b.BranchName,
                                IsActive = c.IsActive,
                                ModifiedDate = c.ModifiedDate,
                                CreationDate = c.CreationDate,
                                Attachments = (from d in _context.Attachments where id == d.ModuleId && d.ModuleType == role select d).ToList(),
                            }).OrderByDescending(a=>a.FirstName).ThenByDescending(a=>a.MiddleName).ThenByDescending(a=>a.LastName).ToListAsync();
                return await data;
            }
            catch (DbUpdateException)
            {
                throw;
            }

        }

        public async Task<MerchantEmployeeDTO> GetEmployeeById(long id)
        {

            try
            {
                int role = (int)Module.MerchantEmployee;
                var data = (from c in _context.MerchantEmployees
                            join b in _context.MerchantBranches on c.MerchantBranchId equals b.Id
                            where c.Id == id 
                            select new MerchantEmployeeDTO
                            {
                                Id = c.Id,
                                EnrolmentId = c.EnrolmentId,
                                MerchantBranchId = c.MerchantBranchId,
                                NationalNumber = c.NationalNumber,
                                ProfilePicture = c.ProfilePicture,
                                FirstName = c.FirstName,
                                MiddleName = c.MiddleName,
                                LastName = c.LastName,
                                Mobile = c.Mobile,
                                Phone = c.Phone,
                                MerchantBranchName = b.BranchName,
                                IsActive = c.IsActive,
                                ModifiedDate = c.ModifiedDate,
                                CreationDate = c.CreationDate,
                                Attachments = (from d in _context.Attachments where id == d.ModuleId && d.ModuleType == role select d).ToList(),
                            }).OrderByDescending(a => a.FirstName).ThenByDescending(a => a.MiddleName).ThenByDescending(a => a.LastName).FirstOrDefaultAsync();
                return await data;
            }
            catch (DbUpdateException)
            {
                throw;
            }

        }



    }
}