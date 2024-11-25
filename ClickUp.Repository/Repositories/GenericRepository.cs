using ClickUp.Data.Contexts;
using ClickUp.Data.Entities.MainEntities;
using ClickUp.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ClickUp.Repository.Repositories
{
    public class GenericRepository<T> :IGenericRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;
        protected readonly ClickUpDbContext _context;

        public GenericRepository(ClickUpDbContext context)
        {
            _context = context;
            _collection = _context.GetCollection<T>();

        }

        public async Task<T> GetByIdAsync(ObjectId id)
           => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();


        public async Task<IEnumerable<T>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync();



        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(FilterDefinition<T> oldEntity, UpdateDefinition<T> newEntity)
        {
            await _collection.UpdateOneAsync(oldEntity, newEntity);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<T> GetByNameAsync(string Name)
        {
            var filter = Builders<T>.Filter.Eq(nameof(T), Name);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
