using System.Collections.Generic;
using SweetBakeryQuest.States;

namespace SweetBakeryQuest.Models
{
    public class Quest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public QuestDifficulty Difficulty { get; set; }

        public Dictionary<ProductType, int> RequiredIngredients { get; set; } = new();
        public int RewardGold { get; set; }
        public int RewardReputation { get; set; }

        public bool IsTimed { get; set; } = false;
        public int TimeLeftSeconds { get; set; } = 0;

        public IQuestState CurrentState { get; private set; } = new AvailableState();

        // Метод, який шукають стани та фабрики для перемикання етапів
        public void SetState(IQuestState newState)
        {
            CurrentState = newState;
        }
    }
}