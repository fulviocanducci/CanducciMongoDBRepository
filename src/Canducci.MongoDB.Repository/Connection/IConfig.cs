namespace Canducci.MongoDB.Repository.Connection
{
    public interface IConfig
    {
        string MongoConnectionString { get; }
        string MongoDatabase { get; }
    }
}
