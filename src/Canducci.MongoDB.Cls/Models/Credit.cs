using MongoDB.Bson;
using Canducci.MongoDB.Repository.Connection;
using Canducci.MongoDB.Repository.MongoAttribute;
using Canducci.MongoDB.Repository.Contracts;

namespace Canducci.MongoDB.Cls.Models
{
    [MongoCollectionName("credit")]
    public class Credit
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }

    public abstract class RepositoryCreditImpl :
        Repository<Credit>,
        IRepository<Credit>
    {
        public RepositoryCreditImpl(IConnect connect)
            :base(connect)
        {                
        }
    }

    public sealed class RepositoryCredit : RepositoryCreditImpl
    {
        public RepositoryCredit(IConnect connect) 
            : base(connect)
        {
        }
    }

}
