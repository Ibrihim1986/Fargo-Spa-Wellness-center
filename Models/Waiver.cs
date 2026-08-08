namespace Family_and_Spa_Wellness.Models;

public class Waiver
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string WaiverType { get; set; } = "General";
    public bool IsSigned { get; set; }
    public DateTime? SignedAt { get; set; }
}
