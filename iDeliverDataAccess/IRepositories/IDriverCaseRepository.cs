
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IDriverCaseRepository
    {
        Task<IEnumerable<DriverCase>> GetAll();
        Task<DriverCase?> GetFirstRow();
        Task<DriverCase?> GetLastRow();
        Task<DriverCase?> GetByID(long id);
        Task<IEnumerable<DriverCase>> Find(Expression<Func<DriverCase, bool>> where);
        Task<DriverCase?> FindRow(Expression<Func<DriverCase, bool>> where);
        bool IsExists(Expression<Func<DriverCase, bool>> where);
        Task Add(DriverCase DriverCase);
        Task Update(DriverCase DriverCase);
        Task Delete(DriverCase DriverCase);
        Task<List<DriverCaseDTO>> GetOnlineDrivers();
    }
}