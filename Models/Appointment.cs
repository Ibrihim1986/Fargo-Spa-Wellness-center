namespace Family_and_Spa_Wellness.Models;

public class Appointment
{
    public int Id { get; set; }
    public int ClientId { get; set; } // the account holder / guardian who booked
    public int ServiceId { get; set; }
    public int? ProviderId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = "Upcoming"; // Upcoming/CheckedIn/InProgress/Completed/NoShow/Cancelled

    // US-306: notes left by the provider about this specific visit
    public string? ProviderNotes { get; set; }

    // US-713: set when this appointment is actually for the client's minor
    // child rather than the client themselves
    public int? DependentId { get; set; }

    // US-704/905-style bookkeeping: whether checkout/payment has been completed
    public bool IsPaid { get; set; }
    public string Status { get; set; } = "Upcoming"; // Upcoming/CheckedIn/Completed/NoShow/Cancelled

    // US-712: shared by the two linked appointments in a couples/side-by-side booking (null = not a group booking)
    public Guid? GroupBookingId { get; set; }
}
