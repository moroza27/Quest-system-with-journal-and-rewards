namespace SweetBakeryQuest.Models
{
    public class ShopItem
    {
        public ProductType Type { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Price { get; set; }

        public string Icon { get; set; } = "📦";

        public int DeliveryTimeSeconds { get; set; }
    }
}
