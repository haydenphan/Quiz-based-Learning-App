using DataAccess.Models;

namespace Services.Contracts
{
    public interface IOptionService
    {
        void ValidateAndPrepareOptions(ICollection<Option> options, int correctAnswerIndex, int questionIndex);
    }
}
