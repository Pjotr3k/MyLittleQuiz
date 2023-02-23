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
                user = user.DoesExist(0, loginVM.Login, loginVM.Password, null);

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

                Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);

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

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        //public IActionResult Signup(SignupViewModel signupVM)
        public async Task<IActionResult> Signup(SignupViewModel signupVM)
        {
            User user = new User();

            if (ModelState.IsValid)
            {
                user = user.DoesExist(0, signupVM.Login, null, signupVM.Email, false);

                if(user != null)
                {
                    if (signupVM.Login == user.Login) signupVM.IsLoginTaken = true;
                    if (signupVM.Email == user.Email) signupVM.IsEmailTaken = true;
                    return View(signupVM);
                }

                MyLittleQuiz.Models.User.SignUp(signupVM.Login, signupVM.Password, signupVM.Email);
            }

            return View();
        }
    }
}
