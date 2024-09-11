using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Presentation.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentSystem.Presentation.Areas.User.Controllers
{
    [Area("User")]  // Area adını belirtiyoruz
    [Authorize]
    public class UserController : BaseController
    {

        private readonly IProductService _productService;

        public UserController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> GetProducts()
        {
            var result = await _productService.GetAllAsync();  // SuccessDataResult döner
            if (result.IsSucces)
            {
                SuccesNotyf("Ürünler Listelendi.");
                return View(result.Data);  // Sadece listeyi (result.Data) View'a geç
            }

            // Hata durumunda bir View dönebilirsiniz veya hata mesajı gösterebilirsiniz
            ErrorNotyf("Ürünler Listeleme Başarısız");
            return View("Error");
        }





    }
}
