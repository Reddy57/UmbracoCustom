using AfsCMS.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AfsCMS.Controllers;

public class ProductsController : Controller
{
    // GET: Products
    [Authorize(Roles = "Afs Level1")]
    public ActionResult Index()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Product 1" },
            new() { Id = 2, Name = "Product 2" },
            new() { Id = 3, Name = "Product 3" },
            new() { Id = 4, Name = "Product 4" },
            new() { Id = 5, Name = "Product 5" }
        };
        return View(products);
    }

    public ActionResult Privacy()
    {
        return View();
    }
}