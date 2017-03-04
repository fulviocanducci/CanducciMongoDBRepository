using Canducci.MongoDB.Repository.Connection;

namespace Canducci.MongoDB.Cls.Models
{
    public class Config : IConfig
    {
        public string MongoConnectionString { get; set; } = "mongodb://localhost:27017";

        public string MongoDatabase { get; set; } = "db";
    }
}
