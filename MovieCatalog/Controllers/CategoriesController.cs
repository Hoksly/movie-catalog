using Microsoft.AspNetCore.Mvc;

namespace MovieCatalog.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        { 
            return View();
        }
    }
}