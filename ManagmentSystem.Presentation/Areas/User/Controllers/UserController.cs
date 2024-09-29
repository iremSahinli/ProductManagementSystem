using ManagmentSystem.Business.Services.ProductServices;
using ManagmentSystem.Business.Services.UserProfileServices;
using ManagmentSystem.Business.Services.CategoryServices; // CategoryService eklendi
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using ManagmentSystem.Presentation.Controllers;
using ManagmentSystem.Business.DTOs.ProductDTOs;

namespace ManagmentSystem.Presentation.Areas.User.Controllers
{
    [Area("User")]  // Area adını belirtiyoruz
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IUserProfileService _userProfileService;
        private readonly ICategoryService _categoryService; // CategoryService dependency eklendi
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public UserController(IProductService productService, IUserProfileService userProfileService,
                              ICategoryService categoryService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _productService = productService;
            _userProfileService = userProfileService;
            _categoryService = categoryService;  // Dependency Injection ile _categoryService tanımlandı
            _stringLocalizer = stringLocalizer;
        }

        public async Task<IActionResult> GetProducts()
        {
            var identityUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ID'yi Guid'e dönüştürüyoruz
            if (!Guid.TryParse(identityUserIdString, out Guid identityUserId))
            {
                ErrorNotyf("Geçersiz kullanıcı ID'si.");
                return RedirectToAction("Error", "Home");
            }

            var userProfile = await _userProfileService.GetUserProfileAsync(identityUserIdString);
            if (userProfile == null)
            {
                ErrorNotyf("Ürünler sayfasına gidebilmek için lütfen profil bilgilerinizi eksiksiz doldurunuz ve kaydediniz!");
                return Redirect("https://localhost:7223/User/Create");
            }

            // Ürünleri alıyoruz
            var result = await _productService.GetAllAsync();
            if (result.IsSucces)
            {
                // Kategorileri alıyoruz ve ViewBag ile View'a taşıyoruz
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = categories;

                SuccesNotyf("Ürünler Listelendi.");
                return View(result.Data);  // Ürünleri view'a geçiyoruz
            }

            // Hata durumunda bir View dönebilirsiniz veya hata mesajı gösterebilirsiniz
            ErrorNotyf("Ürünler Listeleme Başarısız");
            return RedirectToAction("Index", "User", new { area = "" });
        }

        public async Task<IActionResult> FilterProductsByCategory(Guid categoryId)
        {
            try
            {
                var identityUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(identityUserIdString, out Guid identityUserId))
                {
                    return Json(new { success = false, message = "Geçersiz kullanıcı ID'si." });
                }

                var userProfile = await _userProfileService.GetUserProfileAsync(identityUserIdString);
                if (userProfile == null)
                {
                    return Json(new { success = false, message = "Profil bilgilerinizi eksiksiz doldurunuz." });
                }

                // Kategoriye göre ürünleri çekiyoruz
                var result = await _productService.GetProductsByCategoryAsync(categoryId);

                if (result.IsSucces && result.Data.Any())
                {
                    return PartialView("_ProductListPartial", result.Data);
                }
                else
                {
                    return PartialView("_ProductListPartial", new List<ProductListDTO>()); // Boş bir liste döndürüyoruz
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }


    }
}
