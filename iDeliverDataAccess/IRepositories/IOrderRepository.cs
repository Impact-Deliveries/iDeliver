
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order?> GetFirstRow();
        Task<Order?> GetLastRow();
        Task<Order> GetByID(long id);
        Task<IEnumerable<Order>> Find(Expression<Func<Order, bool>> where);
        Task<Order?> FindRow(Expression<Func<Order, bool>> where);
        bool IsExists(Expression<Func<Order, bool>> where);
        Task Add(Order Order);
        Task Update(Order Order);
        Task Delete(Order Order);
        Task<List<OrderDTO?>> GetCurrentOrders();
        Task<List<OrderDTO>> GetOrders(NgOrderTable model);
        Task<List<OrderDTO?>> GetNewOrders();
    }
}