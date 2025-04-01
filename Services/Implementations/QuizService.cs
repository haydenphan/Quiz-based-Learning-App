using DataAccess.Models;
using DataAccess;
using Services.Contracts;

namespace Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly QuizDbContext _context;

        public QuizService(QuizDbContext context)
        {
            _context = context;
        }

        public async Task<Quiz> CreateQuizAsync(string quizTitle, string quizDescription, string userId)
        {
            if (string.IsNullOrWhiteSpace(quizTitle))
            {
                throw new ArgumentException("Quiz title is required.", nameof(quizTitle));
            }

            if (string.IsNullOrWhiteSpace(quizDescription))
            {
                throw new ArgumentException("Quiz description is required.", nameof(quizDescription));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID is required.", nameof(userId));
            }

            var quiz = new Quiz
            {
                QuizTitle = quizTitle,
                QuizDescription = quizDescription,
                UserID = userId,
                CreateAt = DateTime.Now,
                Questions = new List<Question>()
            };

            return quiz;
        }

        public async Task<Quiz> GetByIdAsync(int id)
        {
            return await _context.QuizRepository.FindAsync(id);
        }

        public async Task<List<Quiz>> GetByUserIdAsync(string userId)
        {
            return (await _context.QuizRepository.FindByConditionAsync(q => q.UserID == userId,
                q => q.Questions)).ToList();
        }

        public async Task SaveQuizAsync(Quiz quiz)
        {
            await _context.QuizRepository.AddAsync(quiz);
        }
    }
}
