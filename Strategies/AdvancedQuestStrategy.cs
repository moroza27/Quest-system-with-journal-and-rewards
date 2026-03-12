using System;
using System.Collections.Generic;
using System.Linq;
using SweetBakeryQuest.Models;

namespace SweetBakeryQuest.Strategies
{
    public class AdvancedQuestStrategy : IQuestGenerationStrategy
    {
        private Random _random = new Random();

        // Сюжетні масиви для елітних замовлень
        private string[] _customerNames = { "Мер міста", "Ресторатор", "Пані Вікторія", "Шеф-кухар" };
        private string[] _events = { "на весілля", "для бенкету", "на корпоратив", "для VIP-гостей" };

        public Quest GenerateQuest()
        {
            // Беремо рецепти середньої та високої складності
            var advancedRecipes = RecipeCatalog.AllRecipes.Where(r => r.Difficulty != QuestDifficulty.Easy).ToList();

            // Запобіжник: якщо складних ще немає в каталозі
            if (!advancedRecipes.Any()) advancedRecipes = RecipeCatalog.AllRecipes.ToList();

            var randomRecipe = advancedRecipes[_random.Next(advancedRecipes.Count)];

            int amount = _random.Next(2, 5); // Більші об'єми для складних квестів (2-4 шт)
            string customer = _customerNames[_random.Next(_customerNames.Length)];
            string eventName = _events[_random.Next(_events.Length)];

            return new Quest
            {
                Title = $"VIP Замовлення: {customer}",
                Description = $"Потрібно щось особливе {eventName}. Розраховую на вас!",
                Difficulty = randomRecipe.Difficulty,

                // Даємо більше золота (множник 30 замість 20) + щедрий бонус 50 зверху
                RewardGold = (randomRecipe.Ingredients.Count * 30 * amount) + 50,
                RewardReputation = 15 * amount,

                RequiredIngredients = new Dictionary<ProductType, int>
                {
                    // ПРОСИМО ГОТОВИЙ ПРОДУКТ! (наприклад, 3 Торти)
                    { randomRecipe.OutputProduct, amount }
                }
            };
        }
    }
}