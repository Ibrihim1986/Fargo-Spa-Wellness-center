using Family_and_Spa_Wellness.Models;
using Microsoft.EntityFrameworkCore;

namespace Family_and_Spa_Wellness.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServicePricingTier> ServicePricingTiers => Set<ServicePricingTier>();
    public DbSet<ApprovalRequest> ApprovalRequests => Set<ApprovalRequest>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<ProviderAvailability> ProviderAvailabilities => Set<ProviderAvailability>();

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
    }
}
