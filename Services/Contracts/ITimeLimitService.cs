using DataAccess.Models;

namespace Services.Contracts
{
    public interface ITimeLimitService
    {
        Task<List<TimeLimit>> GetAllTimeLimitsAsync();
        Task<TimeLimit> GetTimeLimitByIdAsync(int timeLimitId);
    }
}
