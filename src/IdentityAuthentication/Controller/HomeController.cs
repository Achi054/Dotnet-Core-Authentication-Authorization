using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAuth
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
            => (_userManager, _signInManager) = (userManager, signInManager);

        [Authorize]
        public IActionResult Index(IdentityUser user) => View(user);

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var userIdentity = await _userManager.FindByNameAsync(userName);

            var isValidPassword = await _userManager.CheckPasswordAsync(userIdentity, password);

            if (isValidPassword)
                await _signInManager.SignInAsync(userIdentity, false);

            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            var result = await _userManager.CreateAsync(new IdentityUser(userName), password);

            if (result.Succeeded)
                return View("Login");

            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
