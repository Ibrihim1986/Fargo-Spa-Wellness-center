using Family_and_Spa_Wellness.Models;
using Microsoft.EntityFrameworkCore;

namespace Family_and_Spa_Wellness.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServicePricingTier> ServicePricingTiers => Set<ServicePricingTier>();
    public DbSet<ApprovalRequest> ApprovalRequests => Set<ApprovalRequest>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<ProviderAvailability> ProviderAvailabilities => Set<ProviderAvailability>();

    // US-304/305/307/308
    public DbSet<GiftCard> GiftCards => Set<GiftCard>();
    public DbSet<Fee> Fees => Set<Fee>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Waiver> Waivers => Set<Waiver>();

    // US-709/713
    public DbSet<SavedCard> SavedCards => Set<SavedCard>();
    public DbSet<Dependent> Dependents => Set<Dependent>();
    public DbSet<GuardianConsent> GuardianConsents => Set<GuardianConsent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Appointment>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne<Service>()
            .WithMany()
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Testimonial>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Testimonial>()
            .HasOne<Service>()
            .WithMany()
            .HasForeignKey(t => t.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProviderAvailability>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(a => a.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GiftCard>()
            .HasIndex(g => g.Code)
            .IsUnique();

        modelBuilder.Entity<Dependent>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(d => d.GuardianUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<GuardianConsent>()
            .HasOne<Dependent>()
            .WithMany()
            .HasForeignKey(c => c.DependentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SavedCard>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
