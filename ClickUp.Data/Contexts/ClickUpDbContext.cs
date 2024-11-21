using ClickUp.Data.Entities.IdentityEntities;
using ClickUp.Data.Entities.MainEntities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ClickUp.Data.Contexts
{
    public class ClickUpDbContext
    {
        private readonly IMongoDatabase _database;

        public ClickUpDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("DefaultConnection"));
            _database = client.GetDatabase(configuration["DataBaseName"]);
        }
        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }
        public IMongoCollection<Project> Projects => _database.GetCollection<Project>("Projects");
        public IMongoCollection<ApplicationUser> Users => _database.GetCollection<ApplicationUser>("Users");
        public IMongoCollection<ApplicationRole> Roles => _database.GetCollection<ApplicationRole>("Roles");
    }
}
