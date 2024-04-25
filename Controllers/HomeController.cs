using Microsoft.AspNetCore.Mvc;
using Ucommerce.Web.BackOffice.Pipelines.Product.CreateProduct;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace CustomUITest.Controllers;

public class HomeController() : Controller
{


    /// <summary>
    /// Index of the controller
    /// </summary>
    /// <returns>List of categories of the current catalog</returns>
    public IActionResult Index()
    {
        return View("/Views/Frontpage.cshtml");
    }

}