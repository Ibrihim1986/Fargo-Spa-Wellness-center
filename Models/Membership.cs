using System;

namespace Family_and_Spa_Wellness.Models;

public class Membership
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    // Last date the membership was renewed (null if never)
    public DateTime? LastRenewedDate { get; set; }

    // Navigation (optional)
    public User? User { get; set; }
}
