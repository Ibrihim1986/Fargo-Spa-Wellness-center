namespace Family_and_Spa_Wellness.Models;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }

    // US-713: minimum client age for this service; null = no restriction
    public int? MinimumAge { get; set; }

    // US-708/US-713: whether booking this service requires a signed waiver/consent
    public bool RequiresWaiver { get; set; }
}
