using DataAccess.Models;
using System.Linq.Expressions;

namespace Services.Contracts
{
    public interface IQuestionService
    {
        Task ValidateAndPrepareQuestionAsync(Question question, int correctAnswerIndex, int questionIndex);
        Task AddQuestionToQuizAsync(Quiz quiz, Question question);
        Task<List<Question>> GetQuestions(Expression<Func<Question, bool>> expression,
             params Expression<Func<Question, object>>[] includes);
    }
}
