using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClickUp.Data.Entities.MainEntities
{
    public class BaseEntity
    {
        [BsonId]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    }
}
