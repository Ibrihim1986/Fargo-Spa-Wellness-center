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

    // US-405: waiver type a client must have signed before this service can be performed (null = none required)
    public string? RequiresWaiverType { get; set; }
}
