namespace DataAccess.Models.Game
{
    public class GamePlayer
    {
        public string PlayerName { get; set; }
        public string ConnectionId { get; set; }
        public int LastQuestionScore { get; set; } = 0;
        public int Score { get; set; } = 0;
        //public int Rank { get; set; } = 0;
    }
}
