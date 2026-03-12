using System;
using System.Collections.Generic;
using System.Linq;
using SweetBakeryQuest.Models;

namespace SweetBakeryQuest.Strategies
{
    public class BeginnerQuestStrategy : IQuestGenerationStrategy
    {
        private Random _random = new Random();

        // Сюжетні масиви для початківців
        private string[] _customerNames = { "Олена", "Школяр Денис", "Пан Андрій", "Анна" };
        private string[] _events = { "до сніданку", "на обідню перерву", "для дітей", "просто до чаю" };

        public Quest GenerateQuest()
        {
            // Беремо ТІЛЬКИ легкі рецепти з каталогу
            var easyRecipes = RecipeCatalog.AllRecipes.Where(r => r.Difficulty == QuestDifficulty.Easy).ToList();

            // Якщо раптом легких немає (що навряд), беремо будь-який
            if (!easyRecipes.Any()) easyRecipes = RecipeCatalog.AllRecipes.ToList();

            var randomRecipe = easyRecipes[_random.Next(easyRecipes.Count)];

            int amount = _random.Next(1, 3); // Від 1 до 2 штук
            string customer = _customerNames[_random.Next(_customerNames.Length)];
            string eventName = _events[_random.Next(_events.Length)];

            return new Quest
            {
                Title = $"Замовлення від: {customer}",
                Description = $"Мені потрібна свіжа випічка {eventName}! Зробите?",
                Difficulty = QuestDifficulty.Easy,

                // Нагорода динамічна: залежить від того, скільки інгредієнтів вимагає рецепт
                RewardGold = randomRecipe.Ingredients.Count * 20 * amount,
                RewardReputation = 5 * amount,

                RequiredIngredients = new Dictionary<ProductType, int>
                {
                    // ПРОСИМО ГОТОВИЙ ПРОДУКТ! (наприклад, 1 Хліб)
                    { randomRecipe.OutputProduct, amount }
                }
            };
        }
    }
}