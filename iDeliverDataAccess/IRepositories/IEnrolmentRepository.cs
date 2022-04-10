using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IEnrolmentRepository
    {
        Task<IEnumerable<Enrolment>> GetAll();
        Task<Enrolment?> GetFirstRow();
        Task<Enrolment?> GetLastRow();
        Task<Enrolment?> GetByID(long id);
        Task<IEnumerable<Enrolment>> Find(Expression<Func<Enrolment, bool>> where);
        Task<Enrolment?> FindRow(Expression<Func<Enrolment, bool>> where);
        bool IsExists(Expression<Func<Enrolment, bool>> where);
        Task Add(Enrolment Enrolment);
        Task Update(Enrolment Enrolment);
        Task Delete(Enrolment Enrolment);
    }
}