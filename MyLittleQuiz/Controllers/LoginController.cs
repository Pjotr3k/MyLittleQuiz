using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLittleQuiz.Models;
using MyLittleQuiz.ViewModels;
using System.Security.Claims;

namespace MyLittleQuiz.Controllers
{
    public class LoginController : Controller
    {        
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            User user = new User();
            
            if (ModelState.IsValid)
            {
                user = user.LogIn(loginVM.Login, loginVM.Password);

                if(user == null)
                {
                    TempData["Error"] = "Incorrect cridential. Check your login or password";
                    return View(loginVM);
                }

                var claims = new List<Claim>
                {
                    //new Claim(ClaimTypes., user.UserId.ToString()),
                    new Claim(ClaimTypes.SerialNumber, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { 
                    IsPersistent = false,
                    AllowRefresh = true
                };

                //Console.WriteLine(claims.FirstOrDefault());

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine(ModelState.IsValid);
            Console.WriteLine(loginVM.Login);
            Console.WriteLine(loginVM.Password);
            return View(loginVM);
        }

        [Authorize]
        public async Task<IActionResult> Logout(string actionName = "Index", string controllerName = "Home")
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(actionName, controllerName);
        }
    }
}
