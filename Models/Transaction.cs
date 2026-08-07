namespace Family_and_Spa_Wellness.Models;

public class Transaction
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public decimal ServiceAmount { get; set; }
    public decimal TipAmount { get; set; }
    public int? ProviderId { get; set; } // who the tip is attributed to
    public string PaymentMethod { get; set; } = string.Empty; // Card, Cash, GiftCard
    public string Status { get; set; } = "Pending"; // Pending, Success, Declined
    public string? ReceiptNumber { get; set; }
    public int? GiftCardId { get; set; } // set if partially/fully paid by gift card
    public decimal GiftCardAmountApplied { get; set; }
    public DateTime CreatedAt { get; set; }
}
