﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClickUp.Data.Entities.MainEntities
{
    public class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    }
}
