using AfsCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Web.Common.Controllers;

namespace AfsCMS.Controllers
{
    public class ProductsController : Controller
    
    {    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

        public ProductsController(IBackOfficeSecurityAccessor backOfficeSecurityAccessor)
        {
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        }
        // GET: Products
        public ActionResult Index()
        {
            
            var backOfficeSecurity = _backOfficeSecurityAccessor.BackOfficeSecurity;
            if (backOfficeSecurity.IsAuthenticated())
            {
                var user = backOfficeSecurity.CurrentUser;

                // You now have access to user details
                return Json(new
                {
                    Name = user.Name,
                    Email = user.Email,
                    UserGroups = user.Groups.Select(g => g.Alias).ToList()
                });
            }
            else
            {
                // No user is logged in
                return Json(null);
            }

            
            
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
