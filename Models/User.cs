namespace Family_and_Spa_Wellness.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = ""; // unique
    public string PasswordHash { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Role { get; set; } = ""; // "Client", "Provider", or "Admin"
    public DateTime CreatedAt { get; set; }
    public string? Title { get; set; } // Provider job title, e.g. "Massage Therapist"
    public string? Bio { get; set; } // Provider bio/specialties
    public DateTime? DateOfBirth { get; set; } // US-603: used to determine eligibility for kids' pricing tiers
    public string FullName => $"{FirstName} {LastName}";
}
