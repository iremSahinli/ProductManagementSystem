using Microsoft.AspNetCore.Mvc;

namespace ManagmentSystem.Presentation.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
