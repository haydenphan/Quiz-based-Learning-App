using DataAccess.Models.Game;
using Services.Contracts;

namespace Services.Implementations
{
    public class GameSessionManager : IGameSessionManager
    {
        private static readonly Dictionary<string, GameSession> ActiveGames = new();
        private static readonly Random RandomGenerator = new();

        public Dictionary<string, GameSession> GetActiveGames() => ActiveGames;
        public void AddPlayer(string pinGame, GamePlayer player)
        {
            ActiveGames.TryGetValue(pinGame, out var gameSession);
            if (gameSession != null)
            {
                gameSession.Players.Add(player);
            }
        }
        public GamePlayer GetPlayer(string gamePin, string playerConnectionId)
        {
            var game = GetGame(gamePin);
            if (game == null) return null;
            var player = game.Players.FirstOrDefault(p => p.ConnectionId == playerConnectionId);
            return player;
        }
        public void RemovePlayer(string pinGame, GamePlayer player)
        {
            var game = GetGame(pinGame);
            if (game != null)
            {
                game.Players.Remove(player);
            }
        }
        public string CreateGame(int quizId, int questionNumber, string hostConnectionId, string hostId)
        {
            var gamePin = GenerateUniqueGamePin();

            GameSession gameSession = new GameSession
            {
                HostConnectionId = hostConnectionId,
                HostId = hostId,
                QuizId = quizId,
                NumberQuestion = questionNumber,
                GamePin = gamePin,
                CurrentQuestion = new CurrentQuestion { SortOrder = 1 },
            };
            ActiveGames[gamePin] = gameSession;
            return gameSession.GamePin;
        }

        public GameSession? GetGame(string gamePin) => ActiveGames.TryGetValue(gamePin, out var game) ? game : null;

        public List<GamePlayer> GetPlayers(string pinGame)
        {
            var players = new List<GamePlayer>();
            var game = GetGame(pinGame);
            if (game != null)
            {
                players = game.Players;
            }
            return players;
        }

        /*public void SetPlayersRank(string gamePin)
        {
            var game = GetGame(gamePin);
            if (game == null) return;
            var players = game.Players;
        }*/

        public void JoinGame(string gamePin, string playerName, string connectionId)
        {
            if (ActiveGames.TryGetValue(gamePin, out var game))
            {
                game.Players.Add(new GamePlayer { PlayerName = playerName, ConnectionId = connectionId });
            }
        }

        public bool RemoveGame(string gamePin) => ActiveGames.Remove(gamePin);
        private string GenerateUniqueGamePin()
        {
            string pin;
            do
            {
                pin = RandomGenerator.Next(100000, 999999).ToString(); // Tạo mã PIN 6 chữ số
            } while (ActiveGames.ContainsKey(pin)); // Kiểm tra nếu trùng thì tạo lại

            return pin;
        }


    }
}
