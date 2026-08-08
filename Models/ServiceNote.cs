namespace Family_and_Spa_Wellness.Models;

public class ServiceNote
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ProviderId { get; set; }
    public int? AppointmentId { get; set; }
    public string NoteType { get; set; } = "General"; // TreatmentNote/SkinCondition/ProductUsage/NailArtDetail/Preference/General
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
