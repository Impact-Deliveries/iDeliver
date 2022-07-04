
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IDriverOrderRepository
    {
        Task<IEnumerable<DriverOrder>> GetAll();

        Task<DriverOrder?> GetFirstRow();

        Task<DriverOrder?> GetLastRow();

        Task<DriverOrder?> GetByID(long id);

        Task<IEnumerable<DriverOrder>> Find(Expression<Func<DriverOrder, bool>> where);
        Task<IEnumerable<DriverOrder>> GetDriverOrders(Expression<Func<DriverOrder, bool>> where);

        Task<DriverOrder?> FindRow(Expression<Func<DriverOrder, bool>> where);

        bool IsExists(Expression<Func<DriverOrder, bool>> where);

        Task Add(DriverOrder DriverOrder);

        Task Update(DriverOrder DriverOrder);

        Task Delete(DriverOrder DriverOrder);

        Task<DriverOrder?> GetHoldDriverOrder(long driverID);

        Task<DriverOrder?> GetOrder(long driverOrderID, long driverID);
    }
}