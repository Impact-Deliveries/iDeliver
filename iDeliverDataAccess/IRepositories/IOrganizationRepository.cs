
using IDeliverObjects.DTO;
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IOrganizationRepository
    {
        Task<IEnumerable<Organization>> GetAll();
        Task<Organization?> GetFirstRow();
        Task<Organization?> GetLastRow();
        Task<Organization?> GetByID(long id);
        Task<IEnumerable<Organization>> Find(Expression<Func<Organization, bool>> where);
        Task<Organization?> FindRow(Expression<Func<Organization, bool>> where);
        bool IsExists(Expression<Func<Organization, bool>> where);
        Task Add(Organization Organization);
        Task Update(Organization Organization);
        Task Delete(Organization Organization);
    }
}