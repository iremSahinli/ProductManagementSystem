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
using Microsoft.Extensions.Localization;
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

        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public AdminUserController(IUserProfileService userProfileService, UserManager<IdentityUser> userManager, AppDbContext context, IMailService mailService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _userProfileService = userProfileService;
            _userManager = userManager;
            _context = context;
            _mailService = mailService;
            _stringLocalizer = stringLocalizer;
        }


        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> ListUsers()
        {
            var users = await _userManager.Users.Where(u => u.Email != "admin@admin.com").ToListAsync();


            var userProfiles = await _userProfileService.GetAllUserProfilesAsync();

            var userList = new List<AdminUserListVM>();


            foreach (var user in users)
            {
                var profile = userProfiles.FirstOrDefault(up => up.IdentityUserId == user.Id);

                if (profile != null)
                {
                    var userVm = new AdminUserListVM
                    {
                        Id = profile.Id,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Mail = profile.Mail,
                        LockoutEnabled = user.LockoutEnabled,
                        IsRecordMissing = false
                    };
                    userList.Add(userVm);
                }
                else
                {
                    var userVm = new AdminUserListVM
                    {
                        Id = Guid.Parse(user.Id),
                        FirstName = "Kayıt Yapılmadı",
                        LastName = "Kayıt Yapılmadı",
                        Mail = user.Email,
                        LockoutEnabled = user.LockoutEnabled,
                        IsRecordMissing = true
                    };
                    userList.Add(userVm);
                }
            }

            var message = _stringLocalizer["Users listed successfully."];
            SuccesNotyf(message);
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
            //Email kısmı kontrolü
            var existingUser = await _userManager.FindByEmailAsync(model.Mail);
            if (existingUser != null)
            {
                var message = _stringLocalizer["The user with this email address already exists."];
                ErrorNotyf(message);
                return View(model);
            }

            // IdentityUser nesnesini oluştur
            var newUser = new IdentityUser
            {
                UserName = model.Mail,
                Email = model.Mail,
                EmailConfirmed = true
            };

            string password = PasswordGenerator.GeneratePassword(); //Rastgele şifre oluştur.

            // Kullanıcıyı oluştur
            var result = await _userManager.CreateAsync(newUser, password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description); // Tüm hataları model durumuna ekleyin.
                }
                return View(model); // Hataları döndür.
            }

            // UserProfile nesnesini oluştur
            var userProfile = new UserProfile
            {
                IdentityUserId = newUser.Id, //newUser.Id kullanarak kullanıcıyı bağla.
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                Mail = model.Mail,
                ProfileImage = model.ProfileImage
            };

            // Kullanıcıya şifreyi içeren bir mail gönder
            string subject = "Hesap Bilgileriniz";
            string body = $"Merhaba {model.FirstName} {model.LastName},\n\nHesabınız başarıyla oluşturuldu. Giriş yapmak için kullanıcı adınız: {model.Mail}, şifreniz: {password}";
            await _mailService.SendMailAsync(newUser.Email, subject, body);

            // UserProfile'ı veritabanına kaydet
            _context.UserProfile.Add(userProfile);
            await _context.SaveChangesAsync();
            var message2 = _stringLocalizer["User creation  successful"];
            SuccesNotyf(message2);
            return RedirectToAction("ListUsers", "AdminUser");
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                var message = _stringLocalizer["User profile not found."];
                ErrorNotyf(message);
                return RedirectToAction("ListUsers", "AdminUser");
            }
            Console.WriteLine($"Kullanıcı ID: {id}");





            var userProfile = await _userProfileService.GetUserProfileByIdAsync(id);
            if (userProfile == null || userProfile.IdentityUserId == null)
            {
                var message = _stringLocalizer["The update cannot be performed until the user completes the registration process!"];//Kullanıcı kayıt işlemini tamamlamadan güncelleştirme işlemi gerçekleştirilemez!
                ErrorNotyf(message);
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
            var oldProfileImage = existingUserProfile.ProfileImage; //Mevcut görsel parametresi.

            // Güncellenmiş bilgileri mevcut varlık üzerinde uygulayın
            existingUserProfile.FirstName = model.FirstName;
            existingUserProfile.LastName = model.LastName;
            existingUserProfile.PhoneNumber = model.PhoneNumber;
            existingUserProfile.DateOfBirth = model.DateOfBirth;
            existingUserProfile.Address = model.Address;
            existingUserProfile.Mail = model.Mail;

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

                // Eski görseli silin, eğer "Not Image" değilse
                if (!string.IsNullOrEmpty(oldProfileImage) && oldProfileImage != "Not Image.jpg")
                {
                    var oldFilePath = Path.Combine(uploadsFolder, oldProfileImage);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }



                // Veritabanında dosya adını saklayın
                existingUserProfile.ProfileImage = uniqueFileName;
            }
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


            _context.Update(existingUserProfile);
            await _context.SaveChangesAsync();
            var message = _stringLocalizer["Update successful!"];
            SuccesNotyf(message);
            return RedirectToAction("ListUsers", "AdminUser");
        }

        public async Task<IActionResult> UserDetail(Guid id)
        {
            var userProfile = await _context.UserProfile.FirstOrDefaultAsync(up => up.Id == id);

            if (userProfile == null)
            {
                var message = _stringLocalizer["User not found,please try again"];
                ErrorNotyf(message);
                return RedirectToAction("ListUsers", "AdminUser");
            }

            var model = new AdminUserDetailVM
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Mail = userProfile.Mail,
                PhoneNumber = userProfile.PhoneNumber,
                DateOfBirth = userProfile.DateOfBirth.Value,
                Address = userProfile.Address,
                ProfileImage = userProfile.ProfileImage

            };
            return View(model);
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var userProfile = await _context.UserProfile.FirstOrDefaultAsync(up => up.Id == id);

                    IdentityUser user;
                    if (userProfile != null)
                    {
                        user = await _userManager.FindByIdAsync(userProfile.IdentityUserId);
                        _context.UserProfile.Remove(userProfile);
                    }
                    else
                    {
                        user = await _userManager.FindByIdAsync(id.ToString());
                    }

                    if (user == null)
                    {
                        var message = _stringLocalizer["User not found,please try again"];
                        ErrorNotyf(message);
                        return RedirectToAction("ListUsers", "AdminUser");
                    }


                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        var message2 = _stringLocalizer["An error occurred while deleting the user, try again!"];
                        ErrorNotyf(message2);
                        return RedirectToAction("ListUsers", "AdminUser");
                    }


                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var message3 = _stringLocalizer["User deleted successfully"];
                    SuccesNotyf("Kullanıcı başarıyla silindi");
                    return RedirectToAction("ListUsers", "AdminUser");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    var message4 = _stringLocalizer["Failed"];
                    ErrorNotyf(message4);
                    return RedirectToAction("ListUsers", "AdminUser");
                }
            }
        }



        public async Task<IActionResult> AdminProfile()
        {
            var adminId = _userManager.GetUserId(User);

            var admin = await _context.Admins.Where(a => a.IdentityId == adminId).Select(a => new AdminProfileVM
            {
                Id = a.IdentityId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,

            }).FirstOrDefaultAsync();

            if (admin == null)
            {
                var message = _stringLocalizer["Admin information is not avaible in this system"];
                ErrorNotyf(message);
            }
            return View(admin);

        }













        [HttpPost]
        [ValidateAntiForgeryToken]
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

            if (lockoutEnabled == false)
            {
                user.LockoutEnd = null; // kullanıcı hesabı aktif olur.
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.MaxValue; // süresiz kilitler.
            }

            var result = await _userManager.UpdateAsync(user); // Kullanıcıyı güncelle

            if (result.Succeeded)
            {
                var message = _stringLocalizer["User status updated."];
                return Ok(message);
            }
            else
            {
                var message2 = _stringLocalizer["User status not updated."];
                return BadRequest(message2);
            }
        }

    }

}





















