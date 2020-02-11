using Microsoft.AspNetCore.Mvc;

namespace JavascriptClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult SignIn() => View();
    }
}
