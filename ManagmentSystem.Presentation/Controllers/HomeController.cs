using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace ManagmentSystem.Presentation.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        public HomeController(ILogger<HomeController> logger, IProductService productService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _logger = logger;
            _productService = productService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            if (products.IsSucces)
            {
                var message = _stringLocalizer["Products Listed!"];
                SuccesNotyf(message);
                return View(products.Data);  // Sadece listeyi (result.Data) View'a geç
            }
            var message2 = _stringLocalizer["Product listing faild"];
            ErrorNotyf(message2);
            var message3 = _stringLocalizer["Failed"];
            return View(message3);
            
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
