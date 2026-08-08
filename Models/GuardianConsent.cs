namespace Family_and_Spa_Wellness.Models;

public class GuardianConsent
{
    public int Id { get; set; }
    public int DependentId { get; set; }
    public int GuardianUserId { get; set; }
    public int ServiceId { get; set; }
    public int? AppointmentId { get; set; }
    public string GuardianSignatureName { get; set; } = string.Empty;
    public DateTime SignedAt { get; set; }
}
