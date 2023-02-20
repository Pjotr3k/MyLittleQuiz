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

            //ClaimsPrincipal identity = Thread.CurrentPrincipal as ClaimsPrincipal;

            spvm.UserId = identity.Claims.Where(c => c.Type == ClaimTypes.SerialNumber)
            .Select(c => c.Value).SingleOrDefault();
            spvm.UserName = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
            .Select(c => c.Value).SingleOrDefault();
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