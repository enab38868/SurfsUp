using Microsoft.AspNetCore.Mvc;

namespace SurfsUpProjekt.Controllers
{
    public class BoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
