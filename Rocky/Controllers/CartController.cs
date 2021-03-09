using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModel;
using Rocky.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Rocky.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Shoppingcart> shoppingcartList = new List<Shoppingcart>();
            if (HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart).Count() > 0)
            {
                shoppingcartList = HttpContext.Session.Get<List<Shoppingcart>>(WC.sessionCart);
            }

            List<int> productInCart = shoppingcartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _db.Products.Where(u => productInCart.Contains(u.Id));

            return View(productList);
        }

        [HttpPost,ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction("Summary");
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<Shoppingcart> shoppingCartList = new List<Shoppingcart>();
            if (HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<Shoppingcart>>(WC.sessionCart);
            }

            List<int> productInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productList = _db.Products.Where(u => productInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = productList
            };
            return View(ProductUserVM);
        }

        public IActionResult Remove(int id)
        {
            List<Shoppingcart> shoppingcartList = new List<Shoppingcart>();
            if (HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<Shoppingcart>>(WC.sessionCart).Count() > 0)
            {
                shoppingcartList = HttpContext.Session.Get<List<Shoppingcart>>(WC.sessionCart);
            }

            var shoppingCart = shoppingcartList.FirstOrDefault(u => u.ProductId == id);
            shoppingcartList.Remove(shoppingCart);
            HttpContext.Session.Set(WC.sessionCart, shoppingcartList);

            return RedirectToAction("Index");
        }
    }


}
