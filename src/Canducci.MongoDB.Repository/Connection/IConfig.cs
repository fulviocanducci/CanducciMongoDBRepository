namespace Canducci.MongoDB.Repository.Connection
{
    public interface IConfig
    {
        string MongoConnectionString { get; set; }
        string MongoDatabase { get; set; }
    }
}
