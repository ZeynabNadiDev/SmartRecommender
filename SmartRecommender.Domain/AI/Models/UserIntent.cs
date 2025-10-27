using SmartRecommender.Domain.AI.Models;

public class UserIntent
{
    public string ActionType { get; set; }                 // Ex: "Search", "Compare", "AddToWishlist"
    public string Category { get; set; }                   // Ex: "Laptop", "Shoes"
    public List<string> Keywords { get; set; } = new();    // Extracted semantic tokens
    public IntentFilters Filters { get; set; } = new();    // PriceRange, Brand, Rating, etc.
    public float Confidence { get; set; }                  // AI confidence level

    // Advanced Context
    public string UserId { get; set; }                     // Optional (for personalization)
    public string DeviceType { get; set; }                 // Ex: "Mobile", "Web"
    public string Region { get; set; }                     // Ex: "Tehran", "Mashhad"
    public string Season { get; set; }                     // Ex: "Winter", "Summer"
    public string PurchaseIntent { get; set; }             // Ex: "ImmediateBuy", "Research"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
