using DataAccess.Models;

namespace Services.Contracts
{
    public interface IQuizService
    {
        Task<Quiz> CreateQuizAsync(string quizTitle, string quizDescription, string userId);
        Task SaveQuizAsync(Quiz quiz);
    }
}
