namespace Family_and_Spa_Wellness.Models;

// Presence of a row means the provider marked that hour unavailable;
// absence means the slot is open by default.
public class ProviderAvailability
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public DateTime Date { get; set; }
    public int Hour { get; set; }
    public bool IsAvailable { get; set; }
}
