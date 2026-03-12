using System;
using System.Linq;
using SweetBakeryQuest.Models;

namespace SweetBakeryQuest.Factories
{
    public class QuestFactory
    {
        private Random _random = new();

        // Масиви для красивого сюжетного тексту
        private string[] _customerNames = { "Олена", "Максим", "Пан Андрій", "Анна", "Марія", "Школяр Денис" };
        private string[] _events = { "на День народження", "для корпоративу", "на сімейну вечерю", "до чаю", "на весілля", "як подарунок" };

        public Quest CreateBoardQuest(int playerLevel)
        {
            var availableRecipes = RecipeCatalog.AllRecipes 
                            .Where(r => r.RequiredLevel <= playerLevel)
                            .ToList();

            // 2. Якщо раптом список порожній (про всяк випадок), беремо найпростіші
            if (!availableRecipes.Any())
            {
                availableRecipes = RecipeCatalog.AllRecipes.Where(r => r.RequiredLevel == 1).ToList();
            }

            // 3. Вибираємо випадковий рецепт з доступних
            var randomRecipe = availableRecipes[_random.Next(availableRecipes.Count)];

            // 2. Генеруємо випадкову кількість та сюжет
            int amount = _random.Next(1, 4); // Від 1 до 3 штук випічки
            string customer = _customerNames[_random.Next(_customerNames.Length)];
            string eventName = _events[_random.Next(_events.Length)];

            var quest = new Quest
            {
                Title = $"Замовлення від: {customer}",
                // Ось тут формуємо гарний опис!
                Description = $"Мені дуже потрібна свіжа випічка {eventName}! Допоможете?",
                Difficulty = randomRecipe.Difficulty,
                RewardGold = randomRecipe.Ingredients.Count * 20 * amount, // Нагорода залежить від складності
                RewardReputation = 15 * amount,
                IsTimed = false
            };

            // 3. 🔥 НАЙГОЛОВНІШЕ: Ми вимагаємо від гравця ГОТОВИЙ ПРОДУКТ, а не сировину!
            // Наприклад: ProductType.Cake -> 1 шт.
            quest.RequiredIngredients.Add(randomRecipe.OutputProduct, amount);

            return quest;
        }

        public Quest CreatePhoneQuest(int playerLevel)
        {
            var quest = CreateBoardQuest(playerLevel); // Беремо базу зі звичайного квесту
            quest.Title = "ТЕРМІНОВЕ ЗАМОВЛЕННЯ!";
            quest.IsTimed = true;
            quest.TimeLeftSeconds = _random.Next(30, 60); // Даємо 30-60 секунд на виконання
            quest.RewardGold += 50; // Бонус за швидкість

            return quest;
        }
    }
}