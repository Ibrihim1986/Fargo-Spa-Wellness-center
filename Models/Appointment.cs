namespace Family_and_Spa_Wellness.Models;

public class Appointment
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ServiceId { get; set; }
    public int? ProviderId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = "Upcoming"; // Upcoming/CheckedIn/Completed/NoShow/Cancelled
}
