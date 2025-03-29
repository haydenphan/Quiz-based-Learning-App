using DataAccess.Models;
using DataAccess;
using Services.Contracts;

namespace Services.Implementations
{
    public class TimeLimitService : ITimeLimitService
    {
        private readonly QuizDbContext _context;

        public TimeLimitService(QuizDbContext context)
        {
            _context = context;
        }

        public async Task<List<TimeLimit>> GetAllTimeLimitsAsync()
        {
            var timeLimits = await _context.TimeLimitRepository.FindAllAsync();
            return timeLimits.ToList();
        }

        public async Task<TimeLimit> GetTimeLimitByIdAsync(int timeLimitId)
        {
            return await _context.TimeLimitRepository.FindAsync(timeLimitId);
        }
    }
}
