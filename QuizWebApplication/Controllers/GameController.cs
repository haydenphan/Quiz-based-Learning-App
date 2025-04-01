using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;

namespace QuizWebApplication.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameSessionManager _gameManager;
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;

        public GameController(IGameSessionManager gameManager, IQuizService quizService, IQuestionService questionService)
        {
            _gameManager = gameManager;
            _quizService = quizService;
            _questionService = questionService;
        }
        [HttpGet]
        public IActionResult Join()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Lobby(int? quizId, string gamePin, string connectionId = "")
        {
            if (quizId == null || string.IsNullOrEmpty(gamePin))
            {
                return BadRequest("Quiz ID and Game PIN are required.");
            }
            var quiz = await _quizService.GetByIdAsync(quizId.Value);
            if (quiz == null)
            {
                return NotFound("Quiz id not found.");
            }
            // Kiểm tra gamePin có tồn tại không
            var game = _gameManager.GetGame(gamePin);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.ConnectionId = connectionId;
            ViewBag.GamePin = gamePin;
            ViewBag.IsHost = !string.IsNullOrEmpty(userId) && game.HostId == userId;
            return View(quiz);
        }
        [HttpGet]
        public async Task<IActionResult> MyQuizzes()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return RedirectToPage("/Account/Login");
            }
            var quizzes = await _quizService.GetByUserIdAsync(userIdClaim) ?? new List<Quiz>();
            return View(quizzes);
        }
        [HttpGet]
        public async Task<IActionResult> Game(string gamePin, int? quizId, string connectionId)
        {
            if (quizId == null || string.IsNullOrEmpty(gamePin))
            {
                return BadRequest("Quiz ID and Game PIN are required.");
            }
            var quiz = await _quizService.GetByIdAsync(quizId.Value);
            if (quiz == null)
            {
                return BadRequest("There are no questions in this quiz?");
            }
            // Kiểm tra gamePin có tồn tại không
            var game = _gameManager.GetGame(gamePin);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.QuizId = quizId.Value;
            ViewBag.ConnectionId = connectionId;
            ViewBag.GamePin = gamePin;
            ViewBag.IsHost = !string.IsNullOrEmpty(userId) && game.HostId == userId;
            return View();
        }
    }
}
