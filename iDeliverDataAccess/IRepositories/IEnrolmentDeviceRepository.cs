using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IEnrolmentDeviceRepository
    {
        Task<IEnumerable<EnrolmentDevice>> GetAll();
        Task<EnrolmentDevice?> GetFirstRow();
        Task<EnrolmentDevice?> GetLastRow();
        Task<EnrolmentDevice?> GetByID(long id);
        Task<IEnumerable<EnrolmentDevice>> Find(Expression<Func<EnrolmentDevice, bool>> where);
        Task<EnrolmentDevice?> FindRow(Expression<Func<EnrolmentDevice, bool>> where);
        bool IsExists(Expression<Func<EnrolmentDevice, bool>> where);
        Task Add(EnrolmentDevice enrolmentDevice);
        Task Update(EnrolmentDevice enrolmentDevice);
        Task Delete(EnrolmentDevice enrolmentDevice);
    }
}