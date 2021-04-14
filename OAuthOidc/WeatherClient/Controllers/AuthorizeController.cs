using Microsoft.AspNetCore.Mvc;

namespace WeatherClient.Controllers
{
    public class AuthorizeController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
