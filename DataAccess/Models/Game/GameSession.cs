namespace DataAccess.Models.Game
{
    public class GameSession
    {
        public string HostConnectionId { get; set; }
        public string HostId { get; set; }
        public List<GamePlayer> Players { get; set; } = new List<GamePlayer>();
        public int QuizId { get; set; }
        public int NumberQuestion { get; set; }
        public CurrentQuestion CurrentQuestion { get; set; }
        public string GamePin { get; set; }
        public bool IsStarted { get; set; } = false;
    }
}
