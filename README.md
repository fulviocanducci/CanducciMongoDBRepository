# Canducci MongoDB Repository Core


[![Canducci MongoDB Repository Core](http://i1308.photobucket.com/albums/s610/maryjanexique/highres_99553512_zpssfgw2lhb.jpeg)](https://www.nuget.org/packages/Canducci.MongoDB.Repository.Core/)


[![NuGet Badge](https://buildstats.info/nuget/Canducci.MongoDB.Repository.Core)](https://www.nuget.org/packages/Canducci.MongoDB.Repository.Core/)

## Install Package (NUGET)

To install Canducci MongoDB Repository Core, run the following command in the [Package Manager Console](http://docs.nuget.org/consume/package-manager-console)

```Csharp

PM> Install-Package Canducci.MongoDB.Repository.Core

```

##How to use?

Create in your `appsettings.json` a section:

```Csharp

"MongoDB": {
    "Database": "dbnew",
    "ConnectionStrings": "mongodb://localhost:27017"
}
```
In method `void ConfigureServices(IServiceCollection services)` do:

```Csharp
public void ConfigureServices(IServiceCollection services)
{
    // Add DI e IOC Container
    services.AddSingleton<IConfiguration>(Configuration);

    // Add Repository MongoDB
    // Config
    services.AddScoped<IConfig, Config>();
    // Connect
    services.AddScoped<IConnect, Connect>();
    // Repositories
    services.AddScoped<RepositoryPeopleImpl, RepositoryPeople>();
    
```

these settings are responsible for the connection layer ___Repository___.

___Make a class that represents your Collection in MongoDB___

```Csharp
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Canducci.MongoDB.Repository.MongoAttribute;
using Canducci.MongoDB.Contracts;
using Canducci.MongoDB.Connection;
namespace Web.Models
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
}
```


___Obs:___ It has a ` MongoCollectionName` attribute that has the configuration of the name of your collection in mongo , if by chance not pass he takes the class name.

___Next step will be the creation of `Repository`.___

___Codification:___

```Csharp
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Canducci.MongoDB.Repository.MongoAttribute;
using Canducci.MongoDB.Contracts;
using Canducci.MongoDB.Connection;
namespace Web.Models
{
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
```

###Controller

```Csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using MongoDB.Bson;
using Canducci.MongoDB.Exceptions;
namespace Canducci.MongoDB.Web.Controllers
{
    public class PeoplesController : Controller
    {
        public RepositoryPeopleImpl Repository { get; private set; }        
        public PeoplesController(RepositoryPeopleImpl repository)
        {               
            Repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(Repository.All());
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {               
            return View(await GetFindAsync(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> Create(People people)
        {
            await Repository.AddAsync(people);
            if (people.Id != ObjectId.Empty)
            {
                return RedirectToAction("Edit", new { id = people.Id });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {               
            return View(await GetFindAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> Edit(string id, People people)
        {
            ObjectId _id;
            if (ObjectId.TryParse(id, out _id))
            {
                people.Id = _id;           
                await Repository.EditAsync(x => x.Id == _id, people);
                if (people.Id != ObjectId.Empty)
                {
                    return RedirectToAction("Edit", new { id = people.Id });
                }
            }               
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {               
            return View(await GetFindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, People people)
        {
            ObjectId _id;
            if (ObjectId.TryParse(id, out _id))
            {
                await Repository.DeleteAsync(x => x.Id == _id);
            }
            return RedirectToAction("Index");
        }

        #region helpers
        private async Task<People> GetFindAsync(string id)
        {
            ObjectId _id;
            if (ObjectId.TryParse(id, out _id))
            {
                return await Repository.FindAsync(x => x.Id == _id);
            }
            throw new RepositoryException("Id Invalid");
        }
        #endregion
    }
}

```
