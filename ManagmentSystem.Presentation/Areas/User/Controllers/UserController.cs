using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Business.Services.UserProfileServices;
using ManagmentSystem.Presentation.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace ManagmentSystem.Presentation.Areas.User.Controllers
{
    [Area("User")]  // Area adını belirtiyoruz
    [Authorize]
    public class UserController : BaseController
    {

        private readonly IProductService _productService;
        private readonly IUserProfileService _userProfileService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public UserController(IProductService productService, IUserProfileService userProfileService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _productService = productService;
            _userProfileService = userProfileService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> GetProducts()
        {
            var identityUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ID'yi Guid'e dönüştürüyoruz
            if (!Guid.TryParse(identityUserIdString, out Guid identityUserId))
            {
                ErrorNotyf("Geçersiz kullanıcı ID'si.");
                return RedirectToAction("Error", "Home");  // Hata sayfasına yönlendirme yapabilirsiniz
            }

            var userProfile = await _userProfileService.GetUserProfileAsync(identityUserIdString);
            if (userProfile == null)
            {
                ErrorNotyf("Ürünler sayfasına gidebilmek için lütfen profil bilgilerinizi eksiksiz doldurunuz ve kaydediniz!");
                return Redirect("https://localhost:7223/User/Create"); //Profil tablosuna yölendir. 
            }


            var result = await _productService.GetAllAsync();  // SuccessDataResult döner
            if (result.IsSucces)
            {
                SuccesNotyf("Ürünler Listelendi.");
                return View(result.Data);  // Sadece listeyi (result.Data) View'a geç
            }

            // Hata durumunda bir View dönebilirsiniz veya hata mesajı gösterebilirsiniz
            ErrorNotyf("Ürünler Listeleme Başarısız");
            return RedirectToAction("Index", "User", new { area = "" });

        }





    }
}
