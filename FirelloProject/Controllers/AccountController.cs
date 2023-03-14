using FirelloProject.Models;
using FirelloProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FirelloProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _signInManager= signInManager;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new();
            user.Email= register.Email;
            user.UserName = register.Username;
            user.FullName = register.FullName;
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }
            //add in role

    

            return RedirectToAction("index", "Home");

        }
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {

            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByEmailAsync(login.UsernameorEmail);

            if (user==null)
            {
                user = await _userManager.FindByNameAsync(login.UsernameorEmail);
                if (user==null)
                {
                    ModelState.AddModelError("", "username or password yanlisdir");
                    return View();
                }
            }
          var result= await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe,true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "hesabiniz bloklanib");
                return View(login);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "username or password yanlisdir");
            }
            //sign in
            await _signInManager.SignInAsync(user, true);


            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
    }
}
