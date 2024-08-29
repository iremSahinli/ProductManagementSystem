using ManagmentSystem.Presentation.Models.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentSystem.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                await Console.Out.WriteLineAsync("Login işlemi başarısız");
                return View(model);
            }

            var checkPassword = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!checkPassword.Succeeded)
            {
                await Console.Out.WriteLineAsync("Login işlemi başarısız");
                return View(model);
            }

            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole == null)
            {
                await Console.Out.WriteLineAsync("Login işlemi başarısız");
                return View(model);
            }

            return RedirectToAction("Index", "Home", new { Area = userRole[0].ToString() });
        }
    }
}
