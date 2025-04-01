using DataAccess.Models.Game;
using Microsoft.AspNetCore.SignalR;
using QuizWebApplication.DTO;
using Services.Contracts;

namespace QuizWebApplication.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameSessionManager _gameManager;
        private readonly IQuestionService _questionService;
        public GameHub(IGameSessionManager gameManager, IQuestionService questionService)
        {
            _gameManager = gameManager;
            _questionService = questionService;
        }

        public async Task<string> CreateGame(int quizId, string hostId = "")
        {

            if (string.IsNullOrEmpty(hostId))
            {
                await Clients.Caller.SendAsync("CreateGameFail");
                return "";
            }
            var hostConnectionId = Context.ConnectionId;
            var questions = await _questionService.GetQuestions(q => q.QuizID == quizId);
            var questionNumber = 0;
            if (questions != null && questions.Count() > 0)
            {
                questionNumber = questions.Count();
            }
            var gamePin = _gameManager.CreateGame(quizId, questionNumber, hostConnectionId, hostId);
            // Host tham gia nhóm của game
            await Groups.AddToGroupAsync(Context.ConnectionId, gamePin);
            // Gửi mã PIN cho host
            await Clients.Caller.SendAsync("GameCreated", gamePin, quizId);
            return gamePin;
        }
        public async Task GetConnectionId()
        {
            var connectionId = Context.ConnectionId;
            await Clients.Caller.SendAsync("OnGetConnectionId", connectionId);
        }
        public async Task JoinGame(string gamePin, string playerName)
        {
            // Kiểm tra gamePin có tồn tại không
            var game = _gameManager.GetGame(gamePin);

            if (game == null)
            {
                await Clients.Caller.SendAsync("JoinFailed", "Invalid Game PIN.");
                return;
            }
            if (game.IsStarted)
            {
                await Clients.Caller.SendAsync("JoinFailed", "The game has begun.");
                return;
            }
            // Kiểm tra nickname có bị trùng không
            var players = game.Players;
            if (players.Any(pl => pl.PlayerName == playerName))
            {
                await Clients.Caller.SendAsync("JoinFailed", "Nickname is already taken.");
                return;
            }
            var playerConnectionId = Context.ConnectionId;
            var newPlayer = new GamePlayer
            {
                PlayerName = playerName,
                ConnectionId = playerConnectionId,
            };
            // Thêm người chơi vào game
            await Groups.AddToGroupAsync(newPlayer.ConnectionId, gamePin);
            _gameManager.AddPlayer(gamePin, newPlayer);
            // Gửi thông báo thành công cho caller
            await Clients.Caller.SendAsync("JoinSuccess", gamePin, game.QuizId, playerConnectionId);
            //await Clients.All.SendAsync("UpdatePlayerList", updatedPlayers);
        }

        public async Task StartGame(string gamepPin)
        {
            var game = _gameManager.GetGame(gamepPin);
            if (game.Players.Count() <= 0)
            {
                return;
            }
            if (game == null)
            {
                await Clients.Caller.SendAsync("StartGameFailed", "C");
                return;
            }
            if (game.HostConnectionId != Context.ConnectionId)
            {
                await Clients.Caller.SendAsync("StartGameFailed", "Only host can start game.");
                return;
            }
            game.IsStarted = true;
            await Clients.Group(gamepPin).SendAsync("StartGame");
        }
        public async Task RemovePlayer(string gamePin = "", string connectionId = "")
        {
            var game = _gameManager.GetGame(gamePin);
            if (game == null)
            {
                await Clients.Caller.SendAsync("RemoveFailed", "Invalid Game PIN.");
                return;
            }
            var hostConnectionId = Context.ConnectionId;
            if (game.HostConnectionId != hostConnectionId)
            {
                await Clients.Caller.SendAsync("RemoveFailed", "Only the host can remove players.");
                return;
            }
            var player = _gameManager.GetPlayer(gamePin, connectionId);
            if (player != null)
            {
                await Groups.RemoveFromGroupAsync(player.ConnectionId, gamePin);
                game.Players.Remove(player);
                var updatedPlayers = _gameManager.GetPlayers(gamePin);
                await Clients.Client(game.HostConnectionId).SendAsync("UpdatePlayerList", updatedPlayers);
                await Clients.Client(connectionId).SendAsync("KickedFromGame", "You have been removed from the game.");
            }
            else
            {
                await Clients.Caller.SendAsync("RemoveFailed", "Player not found.");
            }
        }

        public async Task SubmitAnswer(string gamePin, int optionId, int timeLeft = 0)
        {
            if (timeLeft <= 0)
            {
                await Clients.Caller.SendAsync("SubmitFailed", "Time out for answering questions");
                return;
            }
            var game = _gameManager.GetGame(gamePin);
            if (game == null) return;
            var connectionId = Context.ConnectionId;
            if (game.HostConnectionId == connectionId)
            {
                await Clients.Caller.SendAsync("SubmitFailed", "Host cannot submit.");
                return;
            }
            var player = _gameManager.GetPlayer(gamePin, connectionId);
            if (optionId == game.CurrentQuestion.CorrectOptionId)
            {
                // Tính điểm: tối đa 1000, dựa trên thời gian còn lại
                int score = (int)(1000 * ((float)timeLeft / game.CurrentQuestion.Duration));

                player.LastQuestionScore = Math.Max(score, 100); // Điểm tối thiểu 100 nếu đúng
                player.Score += player.LastQuestionScore; // Cộng vào tổng điểm
            }
            else
            {
                player.LastQuestionScore = 0; // Sai thì điểm câu này là 0
            }
            await Clients.Caller.SendAsync("SubmitSuccess");
        }

        public async Task EndQuestion(string gamePin)
        {
            var game = _gameManager.GetGame(gamePin);
            if (game == null) return;
            if (game.HostConnectionId != Context.ConnectionId) return;
            var players = game.Players.OrderByDescending(p => p.Score);
            await Clients.Client(game.HostConnectionId).SendAsync("HostResult", new
            {
                Result = players,
                CorrectOption = game.CurrentQuestion.CorrectOptionContent,
            });

            int i = 1;
            foreach (var player in players)
            {
                await Clients.Client(player.ConnectionId).SendAsync("PlayerResult", new
                {

                    Rank = i,
                    Score = player.LastQuestionScore,
                    TotalScore = player.Score,
                    CorrectOption = game.CurrentQuestion.CorrectOptionContent,
                });
                i++;
            }
        }

        public async Task NextQuestion(string gamePin)
        {
            var game = _gameManager.GetGame(gamePin);
            if (game == null)
            {
                await Clients.Caller.SendAsync("NextQuestionFailed", "Invalid Game Pin.");
                return;
            }
            if (game.HostConnectionId != Context.ConnectionId)
            {
                await Clients.Caller.SendAsync("NextQuestionFailed", "You're not host.");
                return;
            }
            if (game.CurrentQuestion.SortOrder == game.NumberQuestion)
            {
                await Clients.Group(gamePin).SendAsync("GetFinalResult");
            }
            else
            {
                game.CurrentQuestion = new CurrentQuestion
                {
                    SortOrder = game.CurrentQuestion.SortOrder + 1,
                };
                var players = game.Players;
                foreach (var player in players)
                {
                    player.LastQuestionScore = 0;
                }
                await Clients.Group(gamePin).SendAsync("NextQuestion");
            }
        }
        public async Task GetFinalResult(string gamePin)
        {
            var game = _gameManager.GetGame(gamePin);
            if (game == null)
            {
                await Clients.Caller.SendAsync("NextQuestionFailed", "Invalid Game Pin.");
                return;
            }
            var players = game.Players.OrderByDescending(x => x.Score);
            var ranks = players.Select(p => new
            {
                PlayerName = p.PlayerName,
                Score = p.Score,
            }).ToList();
            await Clients.Client(game.HostConnectionId).SendAsync("FinalResultForHost", ranks);
            int rank = 1;
            foreach (var player in players)
            {
                await Clients.Client(player.ConnectionId).SendAsync("FinalResultForPlayer", new
                {
                    Ranks = ranks,
                    Rank = rank,
                });
                rank++;
            }

            _gameManager.RemoveGame(gamePin);
        }
        public async Task UpdateConnection(string gamePin, string userId = "", string connectionId = "")
        {
            var game = _gameManager.GetGame(gamePin);
            if (game == null)
            {
                await Clients.Caller.SendAsync("JoinFailed", "Invalid Game PIN.");
                return;
            }
            var players = game.Players;
            var newConnectionId = Context.ConnectionId;
            if (!string.IsNullOrEmpty(userId) && game.HostId == userId)
            {
                game.HostConnectionId = newConnectionId;
            }
            else
            {
                var player = _gameManager.GetPlayer(gamePin, connectionId);
                if (player == null) return;
                player.ConnectionId = newConnectionId;
                // Gửi thông báo có player join game
                var updatedPlayers = _gameManager.GetPlayers(gamePin);
                await Clients.Caller.SendAsync("UpdateConnectionIdSuccess", newConnectionId);
                await Clients.Client(game.HostConnectionId).SendAsync("UpdatePlayerList", updatedPlayers);

            }
            await Groups.AddToGroupAsync(newConnectionId, gamePin);
            await Clients.Caller.SendAsync("ConnectionUpdated", "Connection updated successfully");
        }

        public async Task GetCurrentQuestion(string gamePin)
        {

            var game = _gameManager.GetGame(gamePin);
            if (game == null)
            {
                await Clients.Caller.SendAsync("Error", "Invalid Game PIN.");
                return;
            }

            var questions = (await _questionService.GetQuestions(q => q.QuizID == game.QuizId
                                && q.SortOrder == game.CurrentQuestion.SortOrder, q => q.Options, q => q.TimeLimit));
            if (questions != null && questions.Count > 0)
            {
                var question = questions.FirstOrDefault();
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                game.CurrentQuestion.QuestionId = question.Id;
                game.CurrentQuestion.Duration = question.TimeLimit.Duration;
                game.CurrentQuestion.CorrectOptionId = correctOption.Id;
                game.CurrentQuestion.CorrectOptionContent = correctOption.OptionContent;

                var questionResponse = new QuestionGame
                {
                    QuestionId = question.Id,
                    QuestionContent = question.QuestionContent,
                    QuizId = question.QuizID,
                    Duration = question.TimeLimit.Duration,
                    SortOrder = question.SortOrder,
                    Options = question.Options.Select(o => new OptionGame
                    {
                        OptionId = o.Id,
                        QuestionID = o.QuestionID,
                        OptionContent = o.OptionContent,
                        IsCorrect = o.IsCorrect,
                        SortOrder = o.SortOrder,

                    }).ToList(),
                };

                await Clients.Caller.SendAsync("DisplayQuestion", questionResponse);
            }
        }
    }
}
