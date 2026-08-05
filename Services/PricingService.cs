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

    // Determine price for a booking: prefer provider-specific tier for the exact duration, then global tier, then fallback to service.Price
    public async Task<decimal> GetPriceForBookingAsync(int serviceId, int? providerId, int durationMinutes)
    {
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
