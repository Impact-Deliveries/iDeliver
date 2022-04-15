
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IDriverDetailsRepository
    {
        Task<IEnumerable<DriverDetail>> GetAll();
        Task<DriverDetail?> GetFirstRow();
        Task<DriverDetail?> GetLastRow();
        Task<DriverDetail?> GetByID(long id);
        Task<DriverDetail?> GetByDeiverID(long id);
        Task<IEnumerable<DriverDetail>> Find(Expression<Func<DriverDetail, bool>> where);
        Task<DriverDetail?> FindRow(Expression<Func<DriverDetail, bool>> where);
        bool IsExists(Expression<Func<DriverDetail, bool>> where);
        Task Add(DriverDetail DriverDetail);
        Task Update(DriverDetail DriverDetail);
        Task Delete(DriverDetail DriverDetail);
    }
}