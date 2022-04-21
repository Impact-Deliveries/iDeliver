
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IMerchantEmployeeRepository
    {
        Task<IEnumerable<MerchantEmployee>> GetAll();
        Task<MerchantEmployee?> GetFirstRow();
        Task<MerchantEmployee?> GetLastRow();
        Task<MerchantEmployee?> GetByID(long id);
        Task<IEnumerable<MerchantEmployee>> Find(Expression<Func<MerchantEmployee, bool>> where);
        Task<MerchantEmployee?> FindRow(Expression<Func<MerchantEmployee, bool>> where);
        bool IsExists(Expression<Func<MerchantEmployee, bool>> where);
        Task Add(MerchantEmployee MerchantEmployee);
        Task Update(MerchantEmployee MerchantEmployee);
        Task Delete(MerchantEmployee MerchantEmployee);
        Task<List<MerchantEmployeeDTO>> GetMerchantEmployeeByBranchId(long id,bool IsActive);
        Task<MerchantEmployeeDTO> GetEmployeeById(long id);
    }
}