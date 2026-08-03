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
    public string FullName => $"{FirstName} {LastName}";
}
