using DataAccess.Models;

namespace Services.Contracts
{
    public interface IQuizService
    {
        public Task<List<Quiz>> GetByUserIdAsync(string userId);
        public Task<Quiz> GetByIdAsync(int id);
    }
}
