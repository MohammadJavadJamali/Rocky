using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            IEnumerable <ApplicationType> applicationTypes = _db.ApplicationTypes;

            return View(applicationTypes);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            if(ModelState.IsValid)
            {
                _db.ApplicationTypes.Add(applicationType);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(applicationType);
        }


        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0 )
            {
                return NotFound();
            }

            var applicationTypeFromDb = _db.ApplicationTypes.Find(id);

            return View(applicationTypeFromDb);
        }

        [HttpPost]
        public IActionResult Edit(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationTypes.Update(applicationType);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(applicationType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var applicationTypeFromDb = _db.ApplicationTypes.Find(id);

            return View(applicationTypeFromDb);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var applicationTypeFromDb = _db.ApplicationTypes.Find(id);

            _db.ApplicationTypes.Remove(applicationTypeFromDb);

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
