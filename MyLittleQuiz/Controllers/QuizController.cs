using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLittleQuiz.Models;
using MyLittleQuiz.ViewModels;
using System.Security.Claims;

namespace MyLittleQuiz.Controllers
{
    
    public class QuizController : Controller
    {
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateQuizViewModel createVM)
        {
            if(!ModelState.IsValid) return View(createVM);
            Quiz quiz = new Quiz();

            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;

            Models.User user = new User();

            user.UserId = Convert.ToInt32(identity.Claims.Where(c => c.Type == ClaimTypes.SerialNumber)
            .Select(c => c.Value).SingleOrDefault());
            user.Login = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
            .Select(c => c.Value).SingleOrDefault();
            user.Email = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
            .Select(c => c.Value).SingleOrDefault();

            if (createVM.Description != null) quiz = quiz.AddQuiz(createVM.Name, user, createVM.Description);
            else quiz = quiz.AddQuiz(createVM.Name, user);

            return View();
        }

        
        public IActionResult Details(string id)
        {
            Models.User user = new User();
            Quiz quiz = new Quiz();

            DetailQuizViewModel detailVM = new DetailQuizViewModel();
            detailVM.IsModerator = false;

            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;
            user.Principal = identity;
            user = user.GetUserByClaims();

            if(!quiz.Moderators.Any(m => m == user)) detailVM.IsModerator = true;

            if (!quiz.IsPublic && !detailVM.IsModerator) RedirectToAction("Index", "Home");

            /*
             * ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;

            if (identity.Identity.IsAuthenticated){
                user.UserId = Convert.ToInt32(identity.Claims.Where(c => c.Type == ClaimTypes.SerialNumber)
            .Select(c => c.Value).SingleOrDefault());
                user.Login = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();
                user.Email = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                .Select(c => c.Value).SingleOrDefault();
            }
            */

            return View();
        }

    }
}
