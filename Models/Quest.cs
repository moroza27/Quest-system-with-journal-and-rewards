using System;
using System.Collections.Generic;

namespace SweetBakeryQuest.Models
{
    public class Quest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public QuestDifficulty Difficulty { get; set; }
        public int RewardGold { get; set; }
        public int RewardReputation { get; set; }

        public Dictionary<ProductType, int> RequiredIngredients { get; set; } = new();

        public bool IsCompleted { get; set; } = false;
    }
}