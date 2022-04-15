
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IMerchantRepository
    {
        Task<IEnumerable<Merchant>> GetAll();
        Task<Merchant?> GetFirstRow();
        Task<Merchant?> GetLastRow();
        Task<Merchant?> GetByID(long id);
        Task<IEnumerable<Merchant>> Find(Expression<Func<Merchant, bool>> where);
        Task<Merchant?> FindRow(Expression<Func<Merchant, bool>> where);
        bool IsExists(Expression<Func<Merchant, bool>> where);
        Task Add(Merchant Merchant);
        Task Update(Merchant Merchant);
        Task Delete(Merchant Merchant);
        
        Task<List<Merchant>> GetMerchants(string? MerchantName , bool? IsActive = true);
    }
}