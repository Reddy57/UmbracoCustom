using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;


namespace AfsCMS.Controllers;

public class OrdersController : RenderController
{
    public OrdersController(ILogger<OrdersController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor):base
        (logger, compositeViewEngine, umbracoContextAccessor)
    {
            
    }
    // GET
    public override IActionResult Index()
    {
        var contentModel = CurrentPage as Orders;


        return CurrentTemplate(contentModel);
    }
}