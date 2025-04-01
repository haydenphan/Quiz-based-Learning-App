using DataAccess.Models;
using System.Linq.Expressions;

namespace Services.Contracts
{
    public interface IQuestionService
    {
        public Task<List<Question>> GetQuestions(Expression<Func<Question, bool>> expression,
            params Expression<Func<Question, object>>[] includes);
    }
}
