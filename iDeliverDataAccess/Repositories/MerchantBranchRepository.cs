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
    internal class MerchantBranchRepository : IMerchantBranchRepository
    {

        private readonly IDeliverDbContext _context;

        public MerchantBranchRepository(IDeliverDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MerchantBranch>> GetAll() =>
            await _context.MerchantBranches.ToListAsync();

        public async Task<MerchantBranch?> GetFirstRow() =>
            await _context.MerchantBranches.OrderBy(o => o.Id).FirstOrDefaultAsync();

        public async Task<MerchantBranch?> GetLastRow() =>
            await _context.MerchantBranches.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

        public async Task<MerchantBranch?> GetByID(long id) =>
            await _context.MerchantBranches.Where(w => w.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<MerchantBranch>> Find(Expression<Func<MerchantBranch, bool>> where) =>
            await _context.MerchantBranches.Where(where).ToListAsync();

        public async Task<MerchantBranch?> FindRow(Expression<Func<MerchantBranch, bool>> where) =>
            await _context.MerchantBranches.Where(where).Include("Merchant").FirstOrDefaultAsync();

        public bool IsExists(Expression<Func<MerchantBranch, bool>> where) =>
             _context.MerchantBranches.Any(where);

        public async Task Add(MerchantBranch MerchantBranch)
        {
            _context.MerchantBranches.Add(MerchantBranch);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task Update(MerchantBranch MerchantBranch)
        {
            _context.Entry(MerchantBranch).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task Delete(MerchantBranch MerchantBranch)
        {
            try
            {
                _context.MerchantBranches.Remove(MerchantBranch);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task<List<MerchantBranchDTO>> GetBranchesByMerchantID(long? MerchantID, bool? IsActive, long? LocationID)
        {

            var data = (from branch in _context.MerchantBranches
                        join merchant in _context.Merchants on branch.MerchantId equals merchant.Id
                        join locations in _context.Locations on branch.LocationId equals locations.Id
                        where MerchantID == merchant.Id
                        && (IsActive == null ? 1 == 1 : branch.IsActive == IsActive)
                         && (LocationID == null || LocationID ==0? 1 == 1 : branch.LocationId == LocationID)
                        select new MerchantBranchDTO
                        {
                            Id = branch.Id ,
                            MerchantId = branch.MerchantId,
                            MerchantName = merchant.MerchantName,
                            BranchName = branch.BranchName,
                            Overview = branch.Overview,
                            BranchPicture = branch.BranchPicture,
                            LocationId = branch.LocationId,
                            LocationName = locations.Address,
                            Address = branch.Address,
                            Latitude = branch.Latitude,
                            Longitude = branch.Longitude,
                            Mobile = branch.Mobile,
                            Phone = branch.Phone,
                            IsActive = branch.IsActive,
                            ModifiedDate = branch.ModifiedDate,
                            CreationDate = branch.CreationDate,

                        });

            return await data.OrderByDescending(a => a.CreationDate).ToListAsync();
        }

        public async Task<List<MerchantBranchDTO>> GetActiveBranches() 
        {

            var data = (from branch in _context.MerchantBranches
                        join merchant in _context.Merchants on branch.MerchantId equals merchant.Id
                        where branch.IsActive== true && merchant.IsActive==true 
                        select new MerchantBranchDTO
                        {
                            Id = branch.Id,
                            MerchantId = branch.MerchantId,
                            MerchantName = merchant.MerchantName,
                            BranchName = branch.BranchName,                           
                            Latitude = branch.Latitude,
                            Longitude = branch.Longitude,
                            Mobile = branch.Mobile,
                            Phone = branch.Phone,
                        });

            return await data.ToListAsync();
        }
        public async Task<MerchantBranchDTO> GetBrancheID(long? Id)
        {
            var moduleType = (int)Module.MerchantBranch;
            var data = await (from branch in _context.MerchantBranches
                        join merchant in _context.Merchants on branch.MerchantId equals merchant.Id
                        join locations in _context.Locations on branch.LocationId equals locations.Id
                        where Id == branch.Id
                        select new MerchantBranchDTO
                        {
                            Id = branch.Id,
                            MerchantId = branch.MerchantId,
                            MerchantName = merchant.MerchantName,
                            BranchName = branch.BranchName,
                            Overview = branch.Overview,
                            BranchPicture = branch.BranchPicture,
                            LocationId = branch.LocationId,
                            LocationName = locations.Address,
                            Address = branch.Address,
                            Latitude = branch.Latitude,
                            Longitude = branch.Longitude,
                            Mobile = branch.Mobile,
                            Phone = branch.Phone,
                            IsActive = branch.IsActive,
                            ModifiedDate = branch.ModifiedDate,
                            CreationDate = branch.CreationDate,
                            DeliveryPriceOffer= branch.DeliveryPriceOffer,
                            DeliveryStatus=branch.DeliveryStatus,
                            DeliveryPrice=(from c in _context.MerchantDeliveryPrices where c.MerchantBranchId== branch.Id select c).ToList(),
                            Attachments = (from d in _context.Attachments where Id == d.ModuleId && d.ModuleType == moduleType select d).ToList(),
                        }).FirstOrDefaultAsync();

            return  data;
        }

    }
}