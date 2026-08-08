namespace Family_and_Spa_Wellness.Models;

public class GiftCard
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // unique redemption code
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public string? PurchaserEmail { get; set; }
    public string? RecipientEmail { get; set; }
    public DateTime IssuedAt { get; set; }
}
