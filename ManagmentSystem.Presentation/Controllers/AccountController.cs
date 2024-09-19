using ManagmentSystem.Business.Services.MailService;
using ManagmentSystem.Presentation.Models.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ManagmentSystem.Presentation.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMailService mailService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var codeToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = codeToken }, protocol: Request.Scheme);
                var mailMessage = $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>";
                await _mailService.SendMailAsync(model.Email, "Confirm your email", mailMessage);

                TempData["ToastMessage"] = "Registration successful! Please check your email.";
                TempData["ToastType"] = "success";
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            TempData["ToastMessage"] = "Registration failed!";
            TempData["ToastType"] = "error";
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                TempData["ToastMessage"] = "Thank you for confirming your email.";
                TempData["ToastType"] = "success";
                return RedirectToAction("Login", "Account");
            }

            TempData["ToastMessage"] = "Error confirming your email.";
            TempData["ToastType"] = "error";
            return View("Error");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    TempData["ToastMessage"] = "Email not confirmed!";
                    TempData["ToastType"] = "warning";

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    await _mailService.SendMailAsync(user.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                if (!result.Succeeded)
                {
                    ErrorNotyf("Email or password failed.");
                }

                else if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);


                    if (roles.Contains("Admin"))
                    {
                        TempData["ToastMessage"] = "Admin Sayfasına Hoş geldiniz";
                        TempData["ToastType"] = "success";
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        SuccesNotyf("Login Succeeded.");
                        TempData["ToastType"] = "success";
                        return RedirectToAction("Index", "User");
                    }
                }
            }

            TempData["ToastMessage"] = "Login İşlemi Hatalı!";
            TempData["ToastType"] = "danger";
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();


            SuccesNotyf("Logout is success");
            return RedirectToAction("Login", "Account");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ErrorNotyf("Your Mail Address not found! Please try again");
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Account", new { token = token, email = email }, Request.Scheme);

                await _mailService.SendMailAsync(email, "Şifre Sıfırlama Bağlantısı",
                    $"Şifrenizi sıfırlamak için <a href='{resetLink}'>buraya tıklayın</a>.");

                TempData["Message"] = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.";
                //SuccesNotyf("Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.");
                //SuccesNotyf("We send password mail your E-mail address");
                return RedirectToAction("WeSendMail", "Account");
            }
        }
        public IActionResult WeSendMail()
        {
            return View();
        }
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
                return RedirectToAction("Error", "Home");

            var model = new ForgotPasswordVM { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Geçersiz istek.";
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Şifreler uyuşmuyor.";
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                var message = _stringLocalizer["New password create successfully"];
               SuccesNotyf(message);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }


    }
}
