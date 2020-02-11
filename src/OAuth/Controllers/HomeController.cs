using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OAuth.Constants;

namespace OAuth.Controllers
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
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "054"),
                new Claim(JwtRegisteredClaimNames.UniqueName, "Sujith Acharya"),
            };

            var securitySignature = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConstants.SecretKey));
            var signedCredential = new SigningCredentials(securitySignature, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                TokenConstants.Issuer,
                TokenConstants.Audience,
                claims, DateTime.Now,
                DateTime.Now.AddHours(1),
                signedCredential);

            var jsonToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(new { access_token = jsonToken });
        }
    }
}