
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAll();
        Task<Location?> GetFirstRow();
        Task<Location?> GetLastRow();
        Task<Location?> GetByID(long id);
        Task<IEnumerable<Location>> Find(Expression<Func<Location, bool>> where);
        Task<Location?> FindRow(Expression<Func<Location, bool>> where);
        bool IsExists(Expression<Func<Location, bool>> where);
        Task Add(Location Location);
        Task Update(Location Location);
        Task Delete(Location Location);
    }
}