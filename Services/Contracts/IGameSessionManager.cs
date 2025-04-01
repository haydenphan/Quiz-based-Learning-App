using DataAccess.Models.Game;
namespace Services.Contracts
{
    public interface IGameSessionManager
    {
        public Dictionary<string, GameSession> GetActiveGames();
        public string CreateGame(int quizId, int questionNumber, string hostConnectionId, string hostId);
        public void AddPlayer(string pinGame, GamePlayer player);
        public List<GamePlayer> GetPlayers(string pinGame);
        public GamePlayer GetPlayer(string pinGame, string playerConnectionId);
        public void RemovePlayer(string pinGame, GamePlayer player);
        public GameSession? GetGame(string gamePin);

        public void JoinGame(string gamePin, string playerName, string connectionId);
        public bool RemoveGame(string gamePin);

        //public void SetPlayersRank(string gamePin);
    }
}
