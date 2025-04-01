namespace DataAccess.Models.Game
{
    public class CurrentQuestion
    {
        public int QuestionId { get; set; }
        public int CorrectOptionId { get; set; }
        public String CorrectOptionContent { get; set; }
        public int SortOrder { get; set; }
        public int Duration { get; set; }

    }
}
