using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLittleQuiz.Models;
using MyLittleQuiz.ViewModels;
using System.Security.Claims;

namespace MyLittleQuiz.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQuizViewModel createVM)
        {
            if(!ModelState.IsValid) return View(createVM);

            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;

            Models.User user = new User();

            user.UserId = Convert.ToInt32(identity.Claims.Where(c => c.Type == ClaimTypes.SerialNumber)
            .Select(c => c.Value).SingleOrDefault());
            user.Login = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
            .Select(c => c.Value).SingleOrDefault();
            user.Email = identity.Claims.Where(c => c.Type == ClaimTypes.Email)
            .Select(c => c.Value).SingleOrDefault();

            if (createVM.Description != null) Quiz.AddQuiz(createVM.Name, user, createVM.Description);
            else Quiz.AddQuiz(createVM.Name, user);

            return View();
        }



    }
}
