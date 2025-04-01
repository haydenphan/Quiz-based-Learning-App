using DataAccess.Models;
using DataAccess.Repos;
using Services.Contracts;

namespace Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IRepository<Quiz> _quizRepository;
        public QuizService(IRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<Quiz> GetByIdAsync(int id)
        {
            return await _quizRepository.FindAsync(id);
        }

        public async Task<List<Quiz>> GetByUserIdAsync(string userId)
        {
            return (await _quizRepository.FindByConditionAsync(q => q.UserID == userId,
                q => q.Questions)).ToList();
        }

    }
}
