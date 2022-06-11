
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IMerchantDeliveryPriceRepository
    {
        Task<IEnumerable<MerchantDeliveryPrice>> GetAll();
        Task<MerchantDeliveryPrice?> GetFirstRow();
        Task<MerchantDeliveryPrice?> GetLastRow();
        Task<MerchantDeliveryPrice?> GetByID(long id);
        Task<IEnumerable<MerchantDeliveryPrice>> Find(Expression<Func<MerchantDeliveryPrice, bool>> where);
        Task<MerchantDeliveryPrice?> FindRow(Expression<Func<MerchantDeliveryPrice, bool>> where);
        bool IsExists(Expression<Func<MerchantDeliveryPrice, bool>> where);
        Task Add(MerchantDeliveryPrice MerchantDeliveryPrice);
        Task Update(MerchantDeliveryPrice MerchantDeliveryPrice);
        Task Delete(MerchantDeliveryPrice MerchantDeliveryPrice);
        Task<List<MerchantDeliveryPriceDTO>> getByBranchID(long BranchID, int status);

    }
}