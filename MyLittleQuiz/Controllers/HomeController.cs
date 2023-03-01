using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLittleQuiz.Models;
using MyLittleQuiz.ViewModels;
using Org.BouncyCastle.Security;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;

namespace MyLittleQuiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult SecretPage(SecretPageViewModel spvm)
        {
            //private readonly ClaimsPrincipal _principal;

            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;
            Models.User user = new Models.User();
            user.Principal = identity;
            user = user.GetUserByClaims();

            //ClaimsPrincipal identity = Thread.CurrentPrincipal as ClaimsPrincipal;

            spvm.UserId = user.UserId.ToString();
            spvm.UserName = user.Login;
            //ClaimsPrincipal _principal = HttpContext.User;

            return View(spvm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}