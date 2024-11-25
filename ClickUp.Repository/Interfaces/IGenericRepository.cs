using ClickUp.Data.Entities.MainEntities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ClickUp.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(ObjectId id);
        Task<T> GetByNameAsync(string Name);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(FilterDefinition<T> oldEntity, UpdateDefinition<T> newEntity);
        Task DeleteAsync(ObjectId id);
    }
}
