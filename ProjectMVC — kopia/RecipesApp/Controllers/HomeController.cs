using Microsoft.AspNetCore.Mvc;

namespace RecipesApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}
