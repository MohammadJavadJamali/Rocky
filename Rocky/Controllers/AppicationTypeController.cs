using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class AppicationTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AppicationTypeController(ApplicationDbContext db)
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
            _db.ApplicationTypes.Add(applicationType);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
