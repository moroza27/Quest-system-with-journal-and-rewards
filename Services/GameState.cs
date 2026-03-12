using SweetBakeryQuest.Models;
using SweetBakeryQuest.Factories;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;

namespace SweetBakeryQuest.Services
{
    public enum PhoneCallState { None, Ringing, Talking }

    public class GameState
    {
        public string BakeryName { get; set; } = "";
        public bool IsGameStarted { get; set; } = false;

        public void StartGame(string name)
        {
            BakeryName = name;
            IsGameStarted = true;

            // 🔥 Тепер квести генеруються тільки ПІСЛЯ натискання кнопки "Грати"
            GenerateNewQuest();
            GenerateNewQuest();
            GenerateNewQuest();

            _gameTimer.Start(); // Запускаємо таймер тільки коли гра почалася
            NotifyStateChanged();
        }

        public int Gold { get; private set; } = 100;
        public int CurrentXP { get; private set; } = 0;

        public int CurrentLevel => CalculateLevel(CurrentXP);
        public int XpForNextLevel => CalculateXpForNextLevel(CurrentLevel);

        public double LevelProgressPercentage
        {
            get
            {
                int xpForCurrentLevel = CalculateXpForNextLevel(CurrentLevel - 1);
                int xpIntoThisLevel = CurrentXP - xpForCurrentLevel;
                int xpNeededForThisLevel = XpForNextLevel - xpForCurrentLevel;
                if (xpNeededForThisLevel <= 0) return 100;
                return Math.Min(100, (double)xpIntoThisLevel / xpNeededForThisLevel * 100);
            }
        }

        public Dictionary<ProductType, int> Inventory { get; private set; } = new();
        public List<Quest> AvailableQuests { get; private set; } = new();
        public event Action? OnChange;

        private readonly QuestFactory _questFactory;
        private System.Timers.Timer _gameTimer;
        private Random _random = new();

        public Quest? IncomingCall { get; private set; }
        public PhoneCallState CallState { get; private set; } = PhoneCallState.None;
        public bool IsPhoneRinging => IncomingCall != null;

        public GameState(QuestFactory questFactory)
        {
            _questFactory = questFactory;

            // Створюємо таймер, але НЕ запускаємо його тут!
            _gameTimer = new System.Timers.Timer(1000);
            _gameTimer.Elapsed += OnGameTick;
            _gameTimer.AutoReset = true;
        }

        private void OnGameTick(object? sender, ElapsedEventArgs e)
        {
            bool needsUpdate = false;

            // Безпечна робота зі списком
            var questsToUpdate = AvailableQuests.ToList();

            foreach (var quest in questsToUpdate)
            {
                if (quest.IsTimed)
                {
                    if (quest.TimeLeftSeconds > 0)
                    {
                        quest.TimeLeftSeconds--;
                        needsUpdate = true;
                    }
                    else
                    {
                        // Якщо час вийшов, квест провалено або видалено
                        quest.CurrentState.FailQuest(quest);
                        AvailableQuests.Remove(quest); // Видаляємо з дошки, якщо час вийшов
                        needsUpdate = true;
                    }
                }
            }

            if (AvailableQuests.Count(q => !q.IsTimed) < 3 && _random.Next(100) < 5)
            {
                var newQuest = _questFactory.CreateBoardQuest(CurrentLevel);
                AvailableQuests.Add(newQuest);
                needsUpdate = true;
            }

            if (CallState == PhoneCallState.None && IncomingCall == null)
            {
                if (_random.Next(100) < 3)
                {
                    IncomingCall = _questFactory.CreatePhoneQuest(CurrentLevel);
                    CallState = PhoneCallState.Ringing;
                    needsUpdate = true;
                }
            }

            if (needsUpdate) NotifyStateChanged();
        }

        public void GenerateNewQuest()
        {
            var newQuest = _questFactory.CreateBoardQuest(CurrentLevel);
            AvailableQuests.Add(newQuest);
            // NotifyStateChanged викликається вже зовні або в тіку
        }

        public void AnswerPhone() { if (CallState == PhoneCallState.Ringing) { CallState = PhoneCallState.Talking; NotifyStateChanged(); } }
        public void AcceptCallOrder() { if (IncomingCall != null && CallState == PhoneCallState.Talking) { AvailableQuests.Add(IncomingCall); IncomingCall = null; CallState = PhoneCallState.None; NotifyStateChanged(); } }
        public void DeclineCallOrder() { IncomingCall = null; CallState = PhoneCallState.None; NotifyStateChanged(); }

        public bool CanCompleteQuest(Quest quest)
        {
            var totalRequiredRawIngredients = new Dictionary<ProductType, int>();
            foreach (var req in quest.RequiredIngredients)
            {
                var recipe = RecipeCatalog.AllRecipes.FirstOrDefault(r => r.OutputProduct == req.Key);
                if (recipe != null)
                {
                    foreach (var ing in recipe.Ingredients)
                    {
                        if (totalRequiredRawIngredients.ContainsKey(ing.Key))
                            totalRequiredRawIngredients[ing.Key] += ing.Value * req.Value;
                        else
                            totalRequiredRawIngredients[ing.Key] = ing.Value * req.Value;
                    }
                }
                else
                {
                    if (totalRequiredRawIngredients.ContainsKey(req.Key))
                        totalRequiredRawIngredients[req.Key] += req.Value;
                    else
                        totalRequiredRawIngredients[req.Key] = req.Value;
                }
            }

            foreach (var rawReq in totalRequiredRawIngredients)
            {
                if (!Inventory.ContainsKey(rawReq.Key) || Inventory[rawReq.Key] < rawReq.Value)
                    return false;
            }
            return true;
        }

        public bool TryCraftAndComplete(Quest quest)
        {
            if (!CanCompleteQuest(quest)) return false;

            foreach (var req in quest.RequiredIngredients)
            {
                var recipe = RecipeCatalog.AllRecipes.FirstOrDefault(r => r.OutputProduct == req.Key);
                if (recipe != null)
                {
                    foreach (var ing in recipe.Ingredients)
                    {
                        Inventory[ing.Key] -= ing.Value * req.Value;
                    }
                }
                else
                {
                    Inventory[req.Key] -= req.Value;
                }
            }
            return true;
        }

        public void CompleteQuest(Quest quest)
        {
            if (TryCraftAndComplete(quest))
            {
                Gold += quest.RewardGold;
                CurrentXP += quest.RewardReputation;
                AvailableQuests.Remove(quest);
                NotifyStateChanged();
            }
        }

        public bool SpendGold(int amount) { if (Gold >= amount) { Gold -= amount; NotifyStateChanged(); return true; } return false; }

        public void AddToInventory(ProductType type, int amount)
        {
            if (Inventory.ContainsKey(type)) Inventory[type] += amount;
            else Inventory.Add(type, amount);
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

        private int CalculateLevel(int xp)
        {
            if (xp < 50) return 1; if (xp < 120) return 2; if (xp < 250) return 3; if (xp < 500) return 4; return 5;
        }

        private int CalculateXpForNextLevel(int level) => level switch { 0 => 0, 1 => 50, 2 => 120, 3 => 250, 4 => 500, _ => 1000 };
    }
}