using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IDriverSchaduleRepository
    {
        Task<IEnumerable<DriverSchadule>> GetAll();
        Task<DriverSchadule?> GetFirstRow();
        Task<DriverSchadule?> GetLastRow();
        Task<DriverSchadule?> GetByID(long id);
        Task<IEnumerable<DriverSchadule>> Find(Expression<Func<DriverSchadule, bool>> where);
        Task<DriverSchadule?> FindRow(Expression<Func<DriverSchadule, bool>> where);
        bool IsExists(Expression<Func<DriverSchadule, bool>> where);
        Task Add(DriverSchadule DriverSchadule);
        Task Update(DriverSchadule DriverSchadule);
        Task Delete(DriverSchadule DriverSchadule);
    }
}