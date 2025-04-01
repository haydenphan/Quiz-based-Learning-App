using DataAccess.Models;
using DataAccess.Repos;
using Services.Contracts;
using System.Linq.Expressions;

namespace Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionRepository;
        public QuestionService(IRepository<Question> questionRepository)
        {
            _questionRepository = questionRepository;
        }
        public async Task<List<Question>> GetQuestions(Expression<Func<Question, bool>> expression, params Expression<Func<Question, object>>[] includes)
        {
            return (await _questionRepository.FindByConditionAsync(expression, includes))
                ?.ToList() ?? new List<Question>();
        }
    }
}
