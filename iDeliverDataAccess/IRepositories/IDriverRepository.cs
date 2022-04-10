
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IDriverRepository
    {
        Task<IEnumerable<Driver>> GetAll();
        Task<Driver?> GetFirstRow();
        Task<Driver?> GetLastRow();
        Task<Driver?> GetByID(long id);
        Task<IEnumerable<Driver>> Find(Expression<Func<Driver, bool>> where);
        Task<Driver?> FindRow(Expression<Func<Driver, bool>> where);
        bool IsExists(Expression<Func<Driver, bool>> where);
        Task Add(Driver Driver);
        Task Update(Driver Driver);
        Task Delete(Driver Driver);
        Task<DriverTableDTO> GetDrivers(NgDriverTableFilter filter, int pageIndex, int pageSize);
    }
}