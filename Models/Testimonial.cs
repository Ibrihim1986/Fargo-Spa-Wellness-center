namespace Family_and_Spa_Wellness.Models;

public class Testimonial
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int Rating { get; set; } // 1-5
    public string ReviewText { get; set; } = "";
    public string ApprovalStatus { get; set; } = "Pending"; // Pending/Approved/Rejected
    public DateTime CreatedAt { get; set; }
}
