using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ManagmentSystem.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : AdminBaseController
    {
        public readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public HomeController(IStringLocalizer<SharedResources> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult Index()
        {
            var message = _stringLocalizer["Success"];
            SuccesNotyf(message);
            return View();
        }
    }
}
