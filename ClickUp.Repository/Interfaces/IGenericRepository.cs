using ClickUp.Data.Entities.MainEntities;

namespace ClickUp.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(string id);
        Task<T> GetByNameAsync(string Name);
        Task<IEnumerable<T>> GetAllAsync();

        Task AddAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }
}
