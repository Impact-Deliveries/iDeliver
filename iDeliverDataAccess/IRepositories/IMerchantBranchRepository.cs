
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IMerchantBranchRepository
    {
        Task<IEnumerable<MerchantBranch>> GetAll();
        Task<MerchantBranch?> GetFirstRow();
        Task<MerchantBranch?> GetLastRow();
        Task<MerchantBranch?> GetByID(long id);
        Task<IEnumerable<MerchantBranch>> Find(Expression<Func<MerchantBranch, bool>> where);
        Task<MerchantBranch?> FindRow(Expression<Func<MerchantBranch, bool>> where);
        bool IsExists(Expression<Func<MerchantBranch, bool>> where);
        Task Add(MerchantBranch MerchantBranch);
        Task Update(MerchantBranch MerchantBranch);
        Task Delete(MerchantBranch MerchantBranch);
        Task<List<MerchantBranchDTO>> GetBranchesByMerchantID(long? MerchantID, bool? IsActive, long? LocationID);
        Task<MerchantBranchDTO> GetBrancheID(long? Id);
    }
}