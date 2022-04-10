
using IDeliverObjects.Objects;
using System.Linq.Expressions;

namespace iDeliverDataAccess.Repositories
{
    public interface IAttachmentRepository
    {
        Task<IEnumerable<Attachment>> GetAll();
        Task<Attachment?> GetFirstRow();
        Task<Attachment?> GetLastRow();
        Task<Attachment?> GetByID(long id);
        Task<IEnumerable<Attachment>> Find(Expression<Func<Attachment, bool>> where);
        Task<Attachment?> FindRow(Expression<Func<Attachment, bool>> where);
        bool IsExists(Expression<Func<Attachment, bool>> where);
        Task Add(Attachment Attachment);
        Task Update(Attachment Attachment);
        Task Delete(Attachment Attachment);
    }
}