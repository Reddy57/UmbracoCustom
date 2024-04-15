using AfsCMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace AfsCMS.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            var products  = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" },
                new Product { Id = 3, Name = "Product 3" }
            };
            return View(products);
        }
        
        public ActionResult Privacy()
        {
            return View();
        }

    }
}
