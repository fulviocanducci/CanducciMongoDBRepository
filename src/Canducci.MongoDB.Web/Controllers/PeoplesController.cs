using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using MongoDB.Bson;

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
            ObjectId _id = Repository.CreateObjectId(id);
            return View(await Repository.FindAsync(x => x.Id == _id));
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
                RedirectToAction("Edit", new { id = people.Id });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ObjectId _id = Repository.CreateObjectId(id);
            return View(await Repository.FindAsync(x => x.Id == _id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public async Task<IActionResult> Edit(string id, People people)
        {
            ObjectId _id = Repository.CreateObjectId(id);
            people.Id = _id; //no serialize correct
            await Repository.EditAsync(x => x.Id == _id, people);
            if (people.Id != ObjectId.Empty)
            {
                RedirectToAction("Edit", new { id = people.Id });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            ObjectId _id = Repository.CreateObjectId(id);
            return View(await Repository.FindAsync(x => x.Id == _id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, People people)
        {
            ObjectId _id = Repository.CreateObjectId(id);
            await Repository.DeleteAsync(x => x.Id == _id);            
            return RedirectToAction("Index");
        }
    }
}
