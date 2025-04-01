using DataAccess;
using DataAccess.Models;
using Services.Contracts;
using System.Linq.Expressions;

namespace Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IOptionService _optionService;
        private readonly ITimeLimitService _timeLimitService;
        private readonly QuizDbContext _context;

        public QuestionService(IOptionService optionService, ITimeLimitService timeLimitService, QuizDbContext context)
        {
            _optionService = optionService;
            _timeLimitService = timeLimitService;
            _context = context;
        }

        public async Task ValidateAndPrepareQuestionAsync(Question question, int correctAnswerIndex, int questionIndex)
        {
            if (string.IsNullOrWhiteSpace(question.QuestionContent))
            {
                throw new ArgumentException($"Question {questionIndex + 1} content is required.");
            }

            if (question.Options == null || question.Options.Count < 2)
            {
                throw new ArgumentException($"Question {questionIndex + 1} must have at least 2 options.");
            }

            if (correctAnswerIndex < 0 || correctAnswerIndex >= question.Options.Count)
            {
                throw new ArgumentException($"Question {questionIndex + 1} must have a correct answer selected.");
            }

            if (question.SortOrder <= 0)
            {
                throw new ArgumentException($"Question {questionIndex + 1} must have a valid SortOrder.");
            }

            _optionService.ValidateAndPrepareOptions(question.Options, correctAnswerIndex, questionIndex);

            var timeLimit = await _timeLimitService.GetTimeLimitByIdAsync(question.TimeLimitID);
            if (timeLimit == null)
            {
                throw new ArgumentException($"Invalid Time Limit selected for Question {questionIndex + 1}.");
            }
            question.TimeLimit = timeLimit;
        }

        public Task AddQuestionToQuizAsync(Quiz quiz, Question question)
        {
            quiz.Questions.Add(question);
            return Task.CompletedTask;
        }

        public async Task<List<Question>> GetQuestions(Expression<Func<Question, bool>> expression, params Expression<Func<Question, object>>[] includes)
        {
            return (await _context.QuestionRepository.FindByConditionAsync(expression, includes))
                ?.ToList() ?? new List<Question>();
        }
    }
}
