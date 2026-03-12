using System.Collections.Generic;

namespace SweetBakeryQuest.Models
{
    public static class RecipeCatalog
    {
        public static List<Recipe> AllRecipes { get; } = new()
        {
            new Recipe {
                Name = "Пшеничний хліб",
                Icon = "🍞",
                Difficulty = QuestDifficulty.Easy,
                OutputProduct = ProductType.Bread, // Результат випікання
                Ingredients = new Dictionary<ProductType, int> { { ProductType.Flour, 2 } },
                RequiredLevel = 1
            },

            new Recipe {
                Name = "Цукрове печиво",
                Icon = "🍪",
                Difficulty = QuestDifficulty.Easy,
                OutputProduct = ProductType.Cookie, // Результат випікання
                Ingredients = new Dictionary<ProductType, int> { { ProductType.Flour, 1 }, { ProductType.Sugar, 2 } },
                RequiredLevel = 1
            },

            new Recipe {
                Name = "Солодкий пиріжок",
                Icon = "🥧",
                Difficulty = QuestDifficulty.Easy,
                OutputProduct = ProductType.Pie, // Результат випікання
                Ingredients = new Dictionary<ProductType, int> { { ProductType.Flour, 2 }, { ProductType.Sugar, 1 } },
                RequiredLevel = 1
            },

            new Recipe {
                Name = "Королівський круасан",
                Icon = "🥐",
                Difficulty = QuestDifficulty.Medium,
                OutputProduct = ProductType.Croissant, // Результат випікання
                Ingredients = new Dictionary<ProductType, int> {
                    { ProductType.Flour, 2 }, { ProductType.Eggs, 1 }, { ProductType.Milk, 1 } },
                RequiredLevel = 2
            },

            new Recipe {
                Name = "Святковий торт",
                Icon = "🎂",
                Difficulty = QuestDifficulty.Medium,
                OutputProduct = ProductType.Cake, // Результат випікання
                Ingredients = new Dictionary<ProductType, int> {
                    { ProductType.Flour, 3 }, { ProductType.Eggs, 2 }, { ProductType.Sugar, 2 }, { ProductType.Milk, 1 } },
                RequiredLevel = 2
            }
        };
    }
}