using ClickUp.Data.Contexts;
using ClickUp.Data.Entities.MainEntities;
using ClickUp.Repository.Interfaces;
using System.Collections;

namespace ClickUp.Repository.Repositories
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ClickUpDbContext _database;
        private Hashtable _repositories;

        public UnitOfWork(ClickUpDbContext database)
        {
            _database = database;
        }
        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            if (_repositories is null)
                _repositories = new Hashtable();

            var key = typeof(T);

            if (!_repositories.ContainsKey(key))
            {
                var Type = typeof(GenericRepository<>);
                var Instance = Activator.CreateInstance(Type.MakeGenericType(typeof(T)), _database);
                _repositories.Add(key, Instance); ;
            }

            return (IGenericRepository<T>)_repositories[key];
        }
    }
}
