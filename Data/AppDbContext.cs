using System;
using Family_and_Spa_Wellness.Models;
using Microsoft.EntityFrameworkCore;

namespace Family_and_Spa_Wellness.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServicePricingTier> ServicePricingTiers => Set<ServicePricingTier>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();
    public DbSet<ProviderAvailability> ProviderAvailabilities => Set<ProviderAvailability>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<ServiceNote> ServiceNotes => Set<ServiceNote>();

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

        modelBuilder.Entity<ServiceNote>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceNote>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(n => n.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceNote>()
            .HasOne<Appointment>()
            .WithMany()
            .HasForeignKey(n => n.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Use fixed, deterministic CreatedAt values to avoid EF Core "pending model changes" caused by
        // non-deterministic default initializers (e.g. DateTime.UtcNow) when seeding with HasData.
        modelBuilder.Entity<Staff>().HasData(
            new Staff { Id = 1, FirstName = "Alice", LastName = "Manager", Email = "alice@fargospa.com", Role = Role.Manager, IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Staff { Id = 2, FirstName = "Bob", LastName = "Reception", Email = "bob@fargospa.com", Role = Role.Reception, IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Staff { Id = 3, FirstName = "Carol", LastName = "Therapist", Email = "carol@fargospa.com", Role = Role.Therapist, IsActive = true, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Staff { Id = 4, FirstName = "Dave", LastName = "Viewer", Email = "dave@fargospa.com", Role = Role.Viewer, IsActive = false, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
