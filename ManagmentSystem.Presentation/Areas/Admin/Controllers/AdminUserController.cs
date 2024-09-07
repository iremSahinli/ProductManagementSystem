using ManagmentSystem.Business.Services.MailService;
using ManagmentSystem.Business.Services.UserProfileServices;
using ManagmentSystem.Domain.Core.Helpers;
using ManagmentSystem.Domain.Entities;
using ManagmentSystem.Domain.Utilities.Concretes;
using ManagmentSystem.Infrastructure.AppContext;
using ManagmentSystem.Presentation.Areas.Admin.Models.CategoryVMs;
using ManagmentSystem.Presentation.Areas.Admin.Models.UserVMs;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ManagmentSystem.Presentation.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AdminUserController : AdminBaseController
    {

        private readonly IUserProfileService _userProfileService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IMailService _mailService;

        public AdminUserController(IUserProfileService userProfileService, UserManager<IdentityUser> userManager, AppDbContext context, IMailService mailService)
        {
            _userProfileService = userProfileService;
            _userManager = userManager;
            _context = context;
            _mailService = mailService;
        }


        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> ListUsers()
        {
            var userProfiles = await _userProfileService.GetAllUserProfilesAsync();
            if (userProfiles is null)
            {
                ErrorNotyf("User List is Empty");
            }
            // Kullanıcıları IdentityUser tablosundan al ve LockoutEnabled bilgilerini ekle
            var userList = new List<AdminUserListVM>();
            foreach (var profile in userProfiles)
            {
                var identityUser = await _userManager.FindByIdAsync(profile.IdentityUserId);

                if (identityUser != null)
                {
                    var userVm = new AdminUserListVM
                    {
                        Id = profile.Id,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Mail = profile.Mail,
                        LockoutEnabled = identityUser.LockoutEnabled
                    };
                    userList.Add(userVm);
                }
            }
            SuccesNotyf("Users listed successfully.");
            return View(userList);
        }

        public IActionResult Create()
        {
            var model = new AdminUserCreateVM();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(AdminUserCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // IdentityUser nesnesini oluştur
            var newUser = new IdentityUser
            {
                UserName = model.Mail,
                Email = model.Mail,
                EmailConfirmed = true
            };

            string password = PasswordGenerator.GeneratePassword(); //Burada passwordGenerator statik yapıdan rastgele şifre ataması yapıyoruz.

            var result = await _userManager.CreateAsync(newUser, password); // Şifre belirleyin
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı oluşturma başarısız.");
            }

            // UserProfile nesnesini oluştur
            var userProfile = new UserProfile
            {

                IdentityUserId = newUser.Id,//newUser alıyoruz.
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                Mail = model.Mail,
                ProfileImage = model.ProfileImage
            };

            //Yeni kullanıcı eklendiğinde oluşturulan şifresi mail olarak gidecek.
            string subject = "Hesap Bilgileriniz: ";
            string body = $"Merhaba {model.FirstName} {model.LastName},\n\nHesabınız başarıyla oluşturuldu.Giriş yapmak için kullanıcıadınız: {model.Mail},\nşifreniz: {password}";
            await _mailService.SendMailAsync(newUser.Email, subject, body);


            _context.UserProfile.Add(userProfile);
            await _context.SaveChangesAsync();


            return RedirectToAction("ListUsers", "AdminUser");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("ListUsers", "AdminUser");
            }
            Console.WriteLine($"Kullanıcı ID: {id}");


            var userProfile = await _userProfileService.GetUserProfileByIdAsync(id);
            if (userProfile == null || userProfile.IdentityUserId == null)
            {
                return RedirectToAction("ListUsers", "AdminUser");
            }

            var user = await _context.Users
                .Where(u => u.Id == userProfile.IdentityUserId)
                .FirstOrDefaultAsync();

            var model = new AdminUserEditVM
            {

                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Mail = user.Email,
                PhoneNumber = userProfile.PhoneNumber,
                DateOfBirth = userProfile.DateOfBirth,
                Address = userProfile.Address,
                ProfileImage = userProfile.ProfileImage

            };
            return View(model);

        }






        [HttpPost]
        public async Task<IActionResult> Edit(AdminUserEditVM model)
        {

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Hata mesajlarını loglayın veya kullanıcıya gösterin
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            var existingUserProfile = await _context.UserProfile
        .AsNoTracking()
        .FirstOrDefaultAsync(up => up.Id == model.Id);

            if (existingUserProfile == null)
            {
                return RedirectToAction("ListUsers", "AdminUser");
            }

            // Güncellenmiş bilgileri mevcut varlık üzerinde uygulayın
            existingUserProfile.FirstName = model.FirstName;
            existingUserProfile.LastName = model.LastName;
            existingUserProfile.PhoneNumber = model.PhoneNumber;
            existingUserProfile.DateOfBirth = model.DateOfBirth;
            existingUserProfile.Address = model.Address;
            existingUserProfile.Mail = model.Mail;
            existingUserProfile.ProfileImage = model.ProfileImage;

            var identityUserId = existingUserProfile.IdentityUserId;
            var identityUser = await _userManager.FindByIdAsync(identityUserId);
            if (identityUser == null)
            {
                return RedirectToAction("ListUsers", "AdminUser");
            }
            identityUser.Email = model.Mail;
            identityUser.UserName = model.Mail;
            identityUser.NormalizedUserName = model.Mail;
            var updatedResult = await _userManager.UpdateAsync(identityUser);

            // Varlığı `DbContext`'e yeniden ekleyin ve güncelleyin
            _context.Update(existingUserProfile);
            await _context.SaveChangesAsync();

            return RedirectToAction("ListUsers", "AdminUser");
        }



        [HttpPost]
        public async Task<IActionResult> UpdateLockoutStatus(Guid userId, bool lockoutEnabled)
        {
            var userProfile = await _userProfileService.GetUserProfileByIdAsync(userId); //Userprofile de arıyorum.
            if (userProfile == null)
            {
                return RedirectToAction("ListUsers", "AdminUser");
            }

            var user = await _userManager.FindByIdAsync(userProfile.IdentityUserId); //IdentityUser tablosuna gidiyorum.
            if (user == null)
            {
                return RedirectToAction("ListUsers", "AdminUser");
            }
            user.LockoutEnabled = lockoutEnabled;

            if (lockoutEnabled)
            {
                user.LockoutEnd = DateTimeOffset.MaxValue; // Kullanıcıyı kalıcı şekilde kilitler.
            }
            else
            {
                user.LockoutEnd = null; // Kilidi kaldırır
            }

            var result = await _userManager.UpdateAsync(user); // Kullanıcıyı güncelle

            if (result.Succeeded)
            {
                return Ok("Kullanıcı durumu güncellendi.");
            }
            else
            {
                return BadRequest("Güncelleme başarısız oldu.");
            }
        }
    }

}





















