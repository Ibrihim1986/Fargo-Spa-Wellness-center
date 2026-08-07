namespace Family_and_Spa_Wellness.Models;

public class Fee
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int? AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty; // e.g. "Late cancellation", "No-show"
    public string Status { get; set; } = "Pending"; // Pending, Waived, Paid
    public DateTime CreatedAt { get; set; }

    public string? WaivedByName { get; set; }
    public DateTime? WaivedAt { get; set; }
}
