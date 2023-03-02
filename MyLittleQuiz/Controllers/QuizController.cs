using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyLittleQuiz.Models;
using MyLittleQuiz.ViewModels;
using System.Security.Claims;

namespace MyLittleQuiz.Controllers
{
    
    public class QuizController : Controller
    {
        public IActionResult Index()
        {
            IndexQuizViewModel iqvm = new IndexQuizViewModel();
            Quiz quiz = new Quiz();
            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;

            quiz.Principal = identity;

            iqvm.quizzes = quiz.GetAllQuizzes();


            return View(iqvm);
        }

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
            user.Principal = identity;

            user = user.GetUserByClaims();


            if (createVM.Description != null) quiz = quiz.AddQuiz(createVM.Name, user, createVM.Description);
            else quiz = quiz.AddQuiz(createVM.Name, user);



            return RedirectToAction("Detail", "Quiz", new { id = quiz.Id });
        }

        [AllowAnonymous]
        public IActionResult Detail(int id)
        {
            Models.User user = new User();
            Quiz quiz = new Quiz();

            DetailQuizViewModel detailVM = new DetailQuizViewModel();
            detailVM.IsModerator = false;

            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;
            user.Principal = identity;
            user = user.GetUserByClaims();
            detailVM.Quiz = quiz.GetQuizById(id);

            

            if(detailVM.Quiz.Moderators.Any(m => m.UserId == user.UserId)) detailVM.IsModerator = true;

            if (detailVM.Quiz.IsPublic || detailVM.IsModerator) return View(detailVM);
                
            return RedirectToAction("Index", "Home");
            
            
        }

        [Authorize]        
        public async Task<IActionResult> DeleteMod(int id, int quizId, Uri previousPage)
        {
            Quiz quiz = new Quiz();
            quiz = quiz.GetQuizById(quizId);
            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;
            quiz.Principal = identity;

            if(quiz.Creator.UserId != id)
            {
                quiz.DeleteModerator(id);                
            }

            return RedirectToAction("Index", "Quiz");

        }

    }
}
