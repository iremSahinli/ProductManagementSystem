using ManagmentSystem.Business.DTOs.UserProfileDTOs;
using ManagmentSystem.Business.Services.UserProfileServices;
using ManagmentSystem.Presentation.Models.AccountVM;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ManagmentSystem.Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUserProfileService userProfileService, IWebHostEnvironment webHostEnvironment)
        {
            _userProfileService = userProfileService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = await _userProfileService.GetUserProfileAsync(identityUserId);

            if (userProfile == null)
            {
                return RedirectToAction("Create");
            }

            return View(userProfile);
        }

        public async Task<IActionResult> ProfileSettings()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = await _userProfileService.GetUserProfileAsync(identityUserId);

            if (userProfile == null)
            {
                return RedirectToAction("Create");
            }

            return View(userProfile);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userProfileDTO = model.Adapt<UserProfileDTO>();
            userProfileDTO.IdentityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads"); Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }
                userProfileDTO.ProfileImage = "/uploads/" + uniqueFileName;
            }






            var result = await _userProfileService.CreateUserAsync(userProfileDTO);

            if (result.IsSucces)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }
    }
}