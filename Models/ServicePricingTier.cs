namespace Family_and_Spa_Wellness.Models;

public class ServicePricingTier
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public int? ProviderId { get; set; } // null = global/service-level tier
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;

    // US-603: kids' pricing tier — applies when the client's age is at or below this value (null = not an age-based tier)
    public int? MaxAge { get; set; }

    // Navigation properties (optional)
    public Service? Service { get; set; }
    public User? Provider { get; set; }
}
