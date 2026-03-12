using SweetBakeryQuest.Models;

namespace SweetBakeryQuest.States
{
    public interface IQuestState
    {
        string StateName { get; }
        bool AcceptQuest(Quest quest);
        bool CompleteQuest(Quest quest);
        bool FailQuest(Quest quest); // НОВЕ: Можливість провалити квест
    }

    // 1. ЗВИЧАЙНІ КВЕСТИ (Дошка): Одразу доступні, можна тільки здати
    public class AvailableState : IQuestState
    {
        public string StateName => "Доступно на дошці";

        public bool AcceptQuest(Quest quest) => false;

        public bool CompleteQuest(Quest quest)
        {
            quest.SetState(new CompletedState());
            return true;
        }

        public bool FailQuest(Quest quest) => false; // Звичайні квести не провалюються
    }

    // 2. ТЕЛЕФОННІ КВЕСТИ: Взяті в роботу, іде час
    public class InProgressState : IQuestState
    {
        public string StateName => "В процесі (Час іде!)";

        public bool AcceptQuest(Quest quest) => false;

        public bool CompleteQuest(Quest quest)
        {
            quest.SetState(new CompletedState());
            return true;
        }

        public bool FailQuest(Quest quest)
        {
            quest.SetState(new FailedState());
            return true;
        }
    }

    // 3. Успіх
    public class CompletedState : IQuestState
    {
        public string StateName => "Завершено";
        public bool AcceptQuest(Quest quest) => false;
        public bool CompleteQuest(Quest quest) => false;
        public bool FailQuest(Quest quest) => false;
    }

    // 4. Час вийшов (Тільки для телефону)
    public class FailedState : IQuestState
    {
        public string StateName => "Провалено";
        public bool AcceptQuest(Quest quest) => false;
        public bool CompleteQuest(Quest quest) => false;
        public bool FailQuest(Quest quest) => false;
    }
}