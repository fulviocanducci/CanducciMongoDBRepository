using Canducci.MongoDB.Repository.Connection;
using Microsoft.Extensions.Configuration;
namespace Canducci.MongoDB.Web.Models
{
    public class Config : IConfig
    {
        public Config(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection("MongoDB");
            MongoConnectionString = section.GetValue<string>("ConnectionStrings");
            MongoDatabase = section.GetValue<string>("Database");
        }
        public string MongoConnectionString { get; set; }
        public string MongoDatabase { get; set; }
    }
}
