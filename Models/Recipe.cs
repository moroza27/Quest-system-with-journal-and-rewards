using System.Collections.Generic;

namespace SweetBakeryQuest.Models
{
    public class Recipe
    {
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public QuestDifficulty Difficulty { get; set; }

        public ProductType OutputProduct { get; set; }
        public int RequiredLevel { get; set; } = 1;

        public Dictionary<ProductType, int> Ingredients { get; set; } = new();
    }
}