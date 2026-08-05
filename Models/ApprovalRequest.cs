using System;

namespace Family_and_Spa_Wellness.Models;

public class ApprovalRequest
{
    public int Id { get; set; }

    // E.g. "Service" or "ServicePricingTier"
    public string EntityType { get; set; } = string.Empty;

    // E.g. "Create", "Update", "Delete"
    public string Action { get; set; } = string.Empty;

    // Target entity id if applicable (for updates/deletes)
    public int? EntityId { get; set; }

    // JSON payload containing the proposed entity state
    public string Payload { get; set; } = string.Empty;

    public int RequestedById { get; set; }
    public string RequestedByEmail { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; }

    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

    public int? ReviewedById { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewComment { get; set; }
}
