using DataAccess.Models;

namespace Services.Contracts
{
    public interface IQuizService
    {
        Task<Quiz> CreateQuizAsync(string quizTitle, string quizDescription, string userId);
        Task SaveQuizAsync(Quiz quiz);
        Task<Quiz> GetByIdAsync(int id);
        Task<List<Quiz>> GetByUserIdAsync(string userId);
    }
}
