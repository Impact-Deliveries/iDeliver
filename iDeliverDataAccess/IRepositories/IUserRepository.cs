using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetFirstRow();
        Task<User?> GetLastRow();
        Task<User?> GetByID(long id);
        Task<IEnumerable<User>> Find(Expression<Func<User, bool>> where);
        Task<User?> FindRow(Expression<Func<User, bool>> where);
        bool IsExists(Expression<Func<User, bool>> where);
        Task Add(User user);
        Task Update(User user);
        Task Delete(User user);
        Task <int> GetuserCount();
    }
}