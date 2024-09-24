using Microsoft.AspNetCore.Mvc;

namespace ManagmentSystem.Presentation.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public async Task<IActionResult> HandleError(int statusCode)
        {
            Console.WriteLine($"Hata kodu: {statusCode}");

            if (statusCode == 404)
            {
                return View("NotFound"); // 404 sayfası
            }
            else if (statusCode == 403)
            {
                return View("AccessDenied"); // Erişim reddedildi sayfası
            }
            else if (statusCode == 500)
            {
                return View("ServerError"); // Sunucu hatası sayfası
            }
            else
            {
                return View("GenericError"); // Diğer hatalar için
            }
        }
    }
}
