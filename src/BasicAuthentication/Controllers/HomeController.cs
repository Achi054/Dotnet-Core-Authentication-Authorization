using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAndAuthorization.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Login()
        {
            var publicClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Sujith Acharya"),
                new Claim(ClaimTypes.Email, "sujith.acharya054@gmail.com")
            };

            var employeeClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Sujith Shrinivas Acharya"),
                new Claim(ClaimTypes.Email, "sujithacharya@eurofins.com")
            };

            var publicIdentity = new ClaimsIdentity(publicClaims, "Public Claim");
            var employeeIdentity = new ClaimsIdentity(employeeClaims, "Employee Claim");

            var claimPrincipal = new ClaimsPrincipal(new[] { publicIdentity, employeeIdentity });

            HttpContext.SignInAsync(claimPrincipal);

            return RedirectToAction("Index");
        }
    }
}