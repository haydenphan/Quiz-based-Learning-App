using DataAccess.Models;

namespace Services.Contracts
{
    public interface IQuestionService
    {
        Task ValidateAndPrepareQuestionAsync(Question question, int correctAnswerIndex, int questionIndex);
        Task AddQuestionToQuizAsync(Quiz quiz, Question question);
    }
}
