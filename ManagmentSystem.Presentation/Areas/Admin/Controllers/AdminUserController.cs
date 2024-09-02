using ManagmentSystem.Business.Services.UserProfileServices;
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

        public AdminUserController(IUserProfileService userProfileService, UserManager<IdentityUser> userManager, AppDbContext context)
        {
            _userProfileService = userProfileService;
            _userManager = userManager;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> ListUsers()
        {
            var users = await _userProfileService.GetAllUserProfilesAsync();
            var usersVm = users.Adapt<List<AdminUserListVM>>();

            return View(usersVm);
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
                Id = id,
                Email = user.Email,
                LockoutEnabled = user.LockoutEnabled



            };
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminUserEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userProfile = await _userProfileService.GetUserProfileByIdAsync(model.Id); //Userprofile de arıyorum.
            if (userProfile == null)
            {
                return RedirectToAction("ListUsers", "AdminUser");
            }

            var user = await _userManager.FindByIdAsync(userProfile.IdentityUserId); //IdentityUser tablosuna gidiyorum.
            if (user == null)
            {
                return RedirectToAction("ListUsers", "AdminUser");
            }

            user.LockoutEnabled = model.LockoutEnabled;

            if (model.LockoutEnabled)
            {
                
                user.LockoutEnd = DateTimeOffset.MaxValue;
            }
            else
            {

                user.LockoutEnd = null;
            }

            var result = await _userManager.UpdateAsync(user); //Kullanıcıyı IdentityUserda güncelle.
            return View(model);



        }








    }
}
