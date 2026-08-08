namespace Family_and_Spa_Wellness.Models;

public class ClientHealthFlag
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string FlagType { get; set; } = "";
    public string? Details { get; set; }
    public bool IsActive { get; set; } = true;
}
