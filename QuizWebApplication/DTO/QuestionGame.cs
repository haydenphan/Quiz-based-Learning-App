namespace QuizWebApplication.DTO
{
    public class QuestionGame
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }
        public string QuestionContent { get; set; }
        public int SortOrder { get; set; }
        public int Duration { get; set; }
        public List<OptionGame> Options { get; set; } = new List<OptionGame>();
    }
    public class OptionGame
    {
        public int OptionId { get; set; }
        public int QuestionID { get; set; }
        public string OptionContent { get; set; }
        public bool IsCorrect { get; set; }
        public int SortOrder { get; set; }
    }
}
