using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using MongoDB.Bson;
using Canducci.MongoDB.Exceptions;
//using Microsoft.Extensions.Localization;

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
