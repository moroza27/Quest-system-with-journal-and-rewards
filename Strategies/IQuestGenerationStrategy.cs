using SweetBakeryQuest.Models;

namespace SweetBakeryQuest.Strategies
{
    public interface IQuestGenerationStrategy
    {
        Quest GenerateQuest();
    }
}