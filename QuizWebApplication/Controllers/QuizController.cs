using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using System.Security.Claims;
using Services.Contracts;

namespace QuizWebApplication.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;
        private readonly ITimeLimitService _timeLimitService;
        private readonly ILogger<QuizController> _logger;

        public QuizController(
            IQuizService quizService,
            IQuestionService questionService,
            ITimeLimitService timeLimitService,
            ILogger<QuizController> logger)
        {
            _quizService = quizService;
            _questionService = questionService;
            _timeLimitService = timeLimitService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.TimeLimits = await _timeLimitService.GetAllTimeLimitsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveQuiz(string quizTitle, string quizDescription, List<Question> questions, List<int> correctAnswerIndices)
        {
            try
            {
                if (questions == null || !questions.Any())
                {
                    TempData["Error"] = "Please add at least one question to save the quiz.";
                    return RedirectToAction("Create");
                }

                if (correctAnswerIndices == null || correctAnswerIndices.Count != questions.Count)
                {
                    TempData["Error"] = "Correct answer indices are missing or do not match the number of questions.";
                    return RedirectToAction("Create");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "Unable to determine the current user. Please log in again.";
                    return RedirectToAction("Create");
                }

                var quiz = await _quizService.CreateQuizAsync(quizTitle, quizDescription, userId);

                for (int i = 0; i < questions.Count; i++)
                {
                    var question = questions[i];
                    var correctAnswerIndex = correctAnswerIndices[i];

                    await _questionService.ValidateAndPrepareQuestionAsync(question, correctAnswerIndex, i);

                    await _questionService.AddQuestionToQuizAsync(quiz, question);
                }

                await _quizService.SaveQuizAsync(quiz);

                TempData["GamePin"] = quiz.Id.ToString();
                return RedirectToAction("QuizCreated");
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving quiz to database.");
                TempData["Error"] = "An error occurred while saving the quiz. Please try again.";
                return RedirectToAction("Create");
            }
        }

        [HttpGet]
        public IActionResult QuizCreated()
        {
            return View();
        }
    }
}