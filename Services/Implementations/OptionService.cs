using DataAccess.Models;
using Services.Contracts;

namespace Services.Implementations
{
    public class OptionService : IOptionService
    {
        public void ValidateAndPrepareOptions(ICollection<Option> options, int correctAnswerIndex, int questionIndex)
        {
            var optionsList = options.ToList();

            for (int j = 0; j < optionsList.Count; j++)
            {
                var option = optionsList[j];
                if (string.IsNullOrWhiteSpace(option.OptionContent))
                {
                    optionsList[j] = null;
                }
                else
                {
                    option.IsCorrect = (j == correctAnswerIndex);

                    if (option.SortOrder <= 0)
                    {
                        throw new ArgumentException($"Option {j + 1} in Question {questionIndex + 1} must have a valid SortOrder.");
                    }
                }
            }

            var filteredOptions = optionsList.Where(opt => opt != null).ToList();
            options.Clear();
            foreach (var option in filteredOptions)
            {
                options.Add(option);
            }
        }
    }
}
