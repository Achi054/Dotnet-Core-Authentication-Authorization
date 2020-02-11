using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    public class SecretController : Controller
    {
        [Authorize]
        [Route("/message")]
        public string Message() => "Secret Message";
    }
}
