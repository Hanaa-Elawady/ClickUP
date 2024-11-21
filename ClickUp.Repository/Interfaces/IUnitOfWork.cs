using ClickUp.Data.Entities.MainEntities;

namespace ClickUp.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;

    }
}
