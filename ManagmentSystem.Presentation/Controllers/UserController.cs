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

        public UserController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
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
