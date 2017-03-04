using MongoDB.Driver;
using System;
namespace Canducci.MongoDB.Connection
{
    public interface IConnect : IDisposable
    {
        IMongoCollection<T> Collection<T>(string CollectionName);
    }
}
