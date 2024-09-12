using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ManagmentSystem.Presentation.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            if (products.IsSucces)
            {
                SuccesNotyf("�r�nler Listelendi.");
                return View(products.Data);  // Sadece listeyi (result.Data) View'a ge�
            }
            ErrorNotyf("�r�nler Listeleme Ba�ar�s�z");
            return View("Sayfa Y�klenirken Hata olu�tu, tekrar deneyiniz.");
            
        }

        


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
