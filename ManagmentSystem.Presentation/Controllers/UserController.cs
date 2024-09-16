using ManagmentSystem.Business.DTOs.UserProfileDTOs;
using ManagmentSystem.Business.Services.UserProfileServices;
using ManagmentSystem.Presentation.Models.AccountVM;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ManagmentSystem.Presentation.Controllers
{
    public class UserController : BaseController
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

            if (model.ProfileImagePath != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads"); Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImagePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImagePath.CopyToAsync(fileStream);
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






        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            Console.WriteLine("GET Request - IdentityUserId: " + identityUserId);

            var userProfile = await _userProfileService.GetUserProfileAsync(identityUserId);

            if (userProfile == null)
            {
                return RedirectToAction("Create");
            }


            var userProfileVM = new UserProfileVM
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PhoneNumber = userProfile.PhoneNumber,
                DateOfBirth = userProfile.DateOfBirth,
                Address = userProfile.Address,
                Mail = userProfile.Mail,
                ProfileImage = userProfile.ProfileImage
            };

            return View(userProfileVM);
        }




        [HttpPost]
        public async Task<IActionResult> Update(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                ErrorNotyf("Model geçersiz!");
                return View(model);
            }

            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Console.WriteLine("POST Request - IdentityUserId: " + identityUserId);

            var userProfileDTO = await _userProfileService.GetUserProfileAsync(identityUserId);

            if (userProfileDTO == null)
            {
                ErrorNotyf("Kullanıcı profili bulunamadı.");
                ModelState.AddModelError("", "User profile not found.");
                return View(model);
            }

            string oldProfileImage = userProfileDTO.ProfileImage; // Eski profil görselini al

            if (model.ProfileImagePath != null && model.ProfileImagePath.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfileImagePath.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImagePath.CopyToAsync(fileStream);
                }

                // Eski görseli silinecek, not image görseli varsa dosyadan silinmez!!
                if (!string.IsNullOrEmpty(oldProfileImage) && oldProfileImage != "Not Image.jpg")
                {
                    var oldFilePath = Path.Combine(uploadsFolder, oldProfileImage);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Yeni görsel ad
                userProfileDTO.ProfileImage = uniqueFileName;
            }

            // Profil bilgilerini güncelle
            userProfileDTO.FirstName = model.FirstName;
            userProfileDTO.LastName = model.LastName;
            userProfileDTO.PhoneNumber = model.PhoneNumber;
            userProfileDTO.DateOfBirth = model.DateOfBirth;
            userProfileDTO.Address = model.Address;
            userProfileDTO.Mail = model.Mail;

            var result = await _userProfileService.UpdateUserAsync(userProfileDTO);

            if (!result.IsSucces)
            {
                ErrorNotyf("Güncelleme başarısız: " + result.Message);
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            SuccesNotyf("Güncelleme başarılı!");

            return RedirectToAction("ProfileSettings");
        }





    }
}