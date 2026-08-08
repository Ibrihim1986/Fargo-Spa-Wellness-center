namespace Family_and_Spa_Wellness.Models;

public class Waiver
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public DateTime? SignedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    // US-708: health disclosure fields captured at signing
    public string? Allergies { get; set; }
    public string? MedicalConditions { get; set; }

    public bool IsSigned => SignedAt.HasValue;
    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
    public string WaiverType { get; set; } = "General";
    public bool IsSigned { get; set; }
    public DateTime? SignedAt { get; set; }
}
