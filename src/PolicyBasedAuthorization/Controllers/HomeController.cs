using System;
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

        [Authorize(Policy = "Claim.Dob")]
        public IActionResult SecretClaim()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRoleAdmin()
        {
            return View();
        }

        public IActionResult Login()
        {
            var publicClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Sujith Acharya"),
                new Claim(ClaimTypes.Email, "sujith.acharya054@gmail.com"),
                new Claim(ClaimTypes.DateOfBirth, DateTime.Parse("13-03-1990").ToString()),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var employeeClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Sujith Shrinivas Acharya"),
                new Claim(ClaimTypes.Email, "sujithacharya@eurofins.com"),
                new Claim(ClaimTypes.DateOfBirth, DateTime.Parse("13-03-1990").ToString()),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var publicIdentity = new ClaimsIdentity(publicClaims, "Public Claim");
            var employeeIdentity = new ClaimsIdentity(employeeClaims, "Employee Claim");

            var claimPrincipal = new ClaimsPrincipal(new[] { publicIdentity, employeeIdentity });

            HttpContext.SignInAsync(claimPrincipal);

            return RedirectToAction("Index");
        }
    }
}