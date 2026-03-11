using System.Collections.Generic;

namespace SweetBakeryQuest.Models
{
    public class Recipe
    {
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public QuestDifficulty Difficulty { get; set; }

        public Dictionary<ProductType, int> Ingredients { get; set; } = new();
    }
}