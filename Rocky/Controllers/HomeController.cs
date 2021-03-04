using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rocky.Utility;
namespace Rocky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Products.Include(u => u.Category).Include(u => u.ApplicationType),
                Categories = _db.Categories
            };

            return View(homeVM);
        }


        public IActionResult Details(int id)
        {
            List<Shoppingcart> shoppingcartList = new List<Shoppingcart>();
            if (HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart).Count() > 0)
            {
                shoppingcartList = HttpContext.Session.Get<List<Shoppingcart>>(WC.sessionCart);
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Products.Include(u => u.Category).Include(u => u.ApplicationType)
                            .Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            }; 

            foreach(var item in shoppingcartList)
            {
                if(item.ProductId == id)
                {
                    detailsVM.ExistsInCart = true;
                }
            }

            return View(detailsVM);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<Shoppingcart> shoppingcartList = new List<Shoppingcart>();
            if(HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart) != null 
                && HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart).Count() > 0)
            {
                shoppingcartList = HttpContext.Session.Get<List<Shoppingcart>>(WC.sessionCart);
            }
            shoppingcartList.Add(new Shoppingcart { ProductId = id });
            HttpContext.Session.Set(WC.sessionCart, shoppingcartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<Shoppingcart> shoppingcartList = new List<Shoppingcart>();
            if (HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart).Count() > 0)
            {
                shoppingcartList = HttpContext.Session.Get<List<Shoppingcart>>(WC.sessionCart);
            }
            var shoppingCartForRemove = shoppingcartList.SingleOrDefault(r => r.ProductId == id);
            if(shoppingCartForRemove != null)
            {
                shoppingcartList.Remove(shoppingCartForRemove);
            }
            HttpContext.Session.Set(WC.sessionCart, shoppingcartList);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
