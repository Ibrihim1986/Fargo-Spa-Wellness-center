namespace Family_and_Spa_Wellness.Models;

public class SavedCard
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty; // opaque token from the payment provider
    public string Brand { get; set; } = string.Empty; // e.g. "Visa"
    public string Last4 { get; set; } = string.Empty;
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public DateTime CreatedAt { get; set; }
}
