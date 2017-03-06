using Microsoft.Extensions.Configuration;
namespace Canducci.MongoDB.Repository.Connection
{
    public class Config : IConfig
    {           
        public Config(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection("MongoDB"); 
            MongoConnectionString = section["ConnectionStrings"];
            MongoDatabase = section["Database"];            
        }
        public Config(string connectionString, string database)
        {
            MongoConnectionString = connectionString;
            MongoDatabase = database;
        }                                          
        public string MongoConnectionString { get; private set; }
        public string MongoDatabase { get; private set; }
    }
}
