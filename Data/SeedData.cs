using Family_and_Spa_Wellness.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Family_and_Spa_Wellness.Data;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.Services.AnyAsync())
        {
            db.Services.AddRange(
                new Service
                {
                    Name = "Swedish Massage",
                    Category = "Massage",
                    Description = "A relaxing full-body massage to ease tension and improve circulation.",
                    DurationMinutes = 60,
                    Price = 90m,
                    IsActive = true,
                },
                new Service
                {
                    Name = "Signature Facial",
                    Category = "Skincare",
                    Description = "A customized facial treatment to cleanse, exfoliate, and hydrate the skin.",
                    DurationMinutes = 45,
                    Price = 75m,
                    IsActive = true,
                },
                new Service
                {
                    Name = "Hot Stone Therapy",
                    Category = "Massage",
                    Description = "Heated stones combined with massage to relax muscles and relieve stress.",
                    DurationMinutes = 75,
                    Price = 110m,
                    IsActive = true,
                });
        }

        if (!await db.Users.AnyAsync(u => u.Role == "Admin"))
        {
            var hasher = new PasswordHasher<User>();
            var admin = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@fargospa.com",
                Phone = "701-555-0100",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
            };
            admin.PasswordHash = hasher.HashPassword(admin, "ChangeMe123!");

            db.Users.Add(admin);
        }

        await db.SaveChangesAsync();
    }
}
