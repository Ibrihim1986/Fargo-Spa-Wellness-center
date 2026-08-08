using Family_and_Spa_Wellness.Data;
using Family_and_Spa_Wellness.Models;
using Microsoft.EntityFrameworkCore;

namespace Family_and_Spa_Wellness.Services;

public class PricingService
{
    private readonly AppDbContext _db;

    public PricingService(AppDbContext db)
    {
        _db = db;
    }

    // Return available tiers for a service (global and provider-specific)
    public async Task<List<ServicePricingTier>> GetTiersForServiceAsync(int serviceId)
    {
        return await _db.Set<ServicePricingTier>()
            .Where(t => t.ServiceId == serviceId && t.IsActive)
            .ToListAsync();
    }

    // Determine price for a booking: prefer a matching kids' tier for the client's age, then provider-specific tier
    // for the exact duration, then global tier, then fallback to service.Price
    public async Task<decimal> GetPriceForBookingAsync(int serviceId, int? providerId, int durationMinutes, int? clientAge = null)
    {
        // US-603: flag/apply kids' pricing tier for child clients
        if (clientAge.HasValue)
        {
            var kidsTier = await _db.Set<ServicePricingTier>()
                .Where(t => t.ServiceId == serviceId && t.IsActive && t.DurationMinutes == durationMinutes
                    && t.MaxAge != null && t.MaxAge >= clientAge
                    && (t.ProviderId == providerId || t.ProviderId == null))
                .OrderByDescending(t => t.ProviderId != null) // prefer a provider-specific kids tier if both exist
                .FirstOrDefaultAsync();
            if (kidsTier is not null)
                return kidsTier.Price;
        }

        if (providerId.HasValue)
        {
            var providerTier = await _db.Set<ServicePricingTier>()
                .Where(t => t.ServiceId == serviceId && t.IsActive && t.ProviderId == providerId && t.DurationMinutes == durationMinutes)
                .FirstOrDefaultAsync();
            if (providerTier is not null)
                return providerTier.Price;
        }

        var globalTier = await _db.Set<ServicePricingTier>()
            .Where(t => t.ServiceId == serviceId && t.IsActive && t.ProviderId == null && t.DurationMinutes == durationMinutes)
            .FirstOrDefaultAsync();
        if (globalTier is not null)
            return globalTier.Price;

        var svc = await _db.Services.FindAsync(serviceId);
        return svc?.Price ?? 0m;
    }
}
