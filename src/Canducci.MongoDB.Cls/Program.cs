using Canducci.MongoDB.Cls.Models;
using Canducci.MongoDB.Repository.Connection;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
namespace Canducci.Cls
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            IConfig config = new Config("mongodb://localhost:27017", "db");
            IConnect connect = new Connect(config);
            //RepositoryCreditImpl rep = new RepositoryCredit(connect);
            RepositoryPeopleImpl rep = new RepositoryPeople(connect);

            bool result = rep.UpdateAll(a => a.Created != null,
                Builders<People>.Update.Set(_ => _.Value, 2));

            var r0 = rep.Query().Select(a => new { a.Id, a.Name });
            var r1 = r0.ToList();
                            

            System.Console.WriteLine($"{result}");

            //Credit model = new Credit();
            //model.Name = "redeTV.com.br"; 
            //rep.Add(model);

            //var _id = rep.CreateObjectId("58ba1fbcaa0ae801dc886dec");
            //Credit model = rep.Find(x => x.Id == _id);
            //model.Name += " Alteração";

            //rep.Edit(x => x.Id == _id, model);

            //var b = rep.Builder()
            //    .Set(a => a.Name, "alterando")


            //rep.Update(x => x.Id == _id, b);

            //System.Console.WriteLine(rep.Count(a => a.Name.Contains("b")));

            //var r = rep.All(a => a.Name.Contains("o"), a => a.Name)
            //    .ToList();

            //foreach (var item in rep.List(a => a.Name))
            //{
            //    System.Console.WriteLine($"{item.Id} - {item.Name}");
            //}

            System.Console.ReadKey();
                                                                     
        }
    }
}
