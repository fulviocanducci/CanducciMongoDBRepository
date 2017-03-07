using MongoDB.Driver;
using System;
namespace Canducci.MongoDB.Repository.Connection
{
    public interface IConnect : IDisposable
    {
        IMongoCollection<T> Collection<T>(string CollectionName);
    }
}
