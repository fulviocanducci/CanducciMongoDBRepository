using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Canducci.MongoDB.Repository.MongoAttribute;
using Canducci.MongoDB.Repository.Contracts;
using Canducci.MongoDB.Repository.Connection;

namespace Canducci.MongoDB.Cls.Models
{   
    [MongoCollectionName("peoples")]       
    public sealed class People
    {
        [BsonRequired()]
        [BsonId()]
        public ObjectId Id { get; set; }

        [BsonRequired()]
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonRequired()]
        [BsonElement("created")]
        public DateTime Created { get; set; }

        [BsonRequired()]
        [BsonElement("value")]        
        public double Value { get; set; }

        [BsonRequired()]
        [BsonElement("active")]
        public bool Active { get; set; }
    }

    public abstract class RepositoryPeopleImpl :
        Repository<People>,
        IRepository<People>
    {
        public RepositoryPeopleImpl(IConnect connect) : base(connect)
        {
        }
    }

    public sealed class RepositoryPeople : 
        RepositoryPeopleImpl
    {
        public RepositoryPeople(IConnect connect) : base(connect)
        {
        }
    }
}
