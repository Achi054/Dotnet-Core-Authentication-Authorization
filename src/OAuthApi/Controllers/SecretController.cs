﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OAuthApi.Controllers
{
    public class SecretController : Controller
    {
        [Authorize]
        public string Message()
        {
            return "Secret Message";
        }
    }
}
