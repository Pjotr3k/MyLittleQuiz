using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLittleQuiz.Models;
using MyLittleQuiz.ViewModels;
using System.Security.Claims;

namespace MyLittleQuiz.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        [AllowAnonymous]
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


            if (identity != null && identity.Identity.IsAuthenticated)
            {
                if (detailVM.Quiz.Moderators.Any(m => m.UserId == user.UserId)) detailVM.IsModerator = true;
            }

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

            return RedirectToAction("Detail", "Quiz", new { id = quizId });

        }

        
        public IActionResult AddPool(int id)
        {
            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;

            User user = new User();
            user.Principal = identity;
            user = user.GetUserByClaims();
            
            Quiz quiz = new Quiz();
            quiz.Principal = identity;
            quiz = quiz.GetQuizById(id);

            AddPoolQuizViewModel apqvm = new AddPoolQuizViewModel();

            if(quiz.Moderators.Any(m => m.UserId == user.UserId))
            {
                apqvm.QuizId = quiz.Id;

                return View(apqvm);
            }

            return RedirectToAction("Detail", "Quiz", id);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddPool(int id, AddPoolQuizViewModel apqvm)
        {
            //Quiz quiz = new Quiz();
            //quiz = quiz.GetQuizById(quizId);
            //ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;
            //dqvm.Quiz.Principal = identity;
            apqvm.QuizId = id;

            if (ModelState.IsValid)
            {
                ScorePool.AddPool(apqvm.Name, apqvm.QuizId);
            }

            return RedirectToAction("Detail", "Quiz", new { id = apqvm.QuizId});

        }

        public IActionResult AddMod(int id)
        {
            ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;

            User user = new User();
            user.Principal = identity;
            user = user.GetUserByClaims();

            Quiz quiz = new Quiz();
            quiz.Principal = identity;
            quiz = quiz.GetQuizById(id);

            AddModQuizViewModel amqvm = new AddModQuizViewModel();
            

            if (quiz.Moderators.Any(m => m.UserId == user.UserId))
            {
                amqvm.Users = Models.User.GetAllUsers();
                amqvm.QuizId = quiz.Id;

                return View(amqvm);
            }

            return RedirectToAction("Detail", "Quiz", id);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMod(int id, AddPoolQuizViewModel amqvm)
        {
            //Quiz quiz = new Quiz();
            //quiz = quiz.GetQuizById(quizId);
            //ClaimsPrincipal identity = HttpContext.User as ClaimsPrincipal;
            //dqvm.Quiz.Principal = identity;
            //amqvm.QuizId = id;
            Models.User user = new User();
            Quiz quiz = new Quiz();
            quiz = quiz.GetQuizById(id);
            user = user.DoesExist(0, amqvm.Name, null, null);

            if (ModelState.IsValid)
            {
                quiz.AddModerator(user.UserId);
            }

            return RedirectToAction("Detail", "Quiz", new { id = id });

        }

    }
}
