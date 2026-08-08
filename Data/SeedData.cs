using Family_and_Spa_Wellness.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Family_and_Spa_Wellness.Data;

public static class SeedData
{
    private const string SeedPasswordHash = "seed-no-login";

    public static async Task SeedAsync(AppDbContext db, IConfiguration configuration)
    {
        await SeedServicesAsync(db);
        await SeedUsersAsync(db, configuration);
        await SeedAppointmentsAsync(db);
        await SeedTestimonialsAsync(db);
        await SeedWaiversAsync(db);
        await SeedHealthFlagsAsync(db);
    }

    private static async Task SeedServicesAsync(AppDbContext db)
    {
        var services = new List<Service>
        {
            new()
            {
                Name = "60-Min Therapeutic Massage",
                Category = "Massage",
                Description = "A restorative full-body massage tailored to release tension and improve circulation. Featured — $79 intro rate.",
                DurationMinutes = 60,
                Price = 79m,
                IsActive = true,
            },
            new()
            {
                Name = "Deep Tissue Massage",
                Category = "Massage",
                Description = "Slow, firm pressure targeting deep muscle layers and connective tissue. Contact for pricing.",
                DurationMinutes = 60,
                Price = 0m,
                IsActive = true,
                RequiresWaiverType = "MassageIntake",
            },
            new()
            {
                Name = "Salt Cave Massage",
                Category = "Massage",
                Description = "A 90-minute massage in our Himalayan salt cave for deep respiratory and muscular relief. Featured.",
                DurationMinutes = 90,
                Price = 390m,
                IsActive = true,
            },
            new()
            {
                Name = "Customized Facial",
                Category = "Facials & Skin Care",
                Description = "A personalized facial tailored to your skin type and concerns. Featured — $37.50 promo price.",
                DurationMinutes = 50,
                Price = 37.50m,
                IsActive = true,
            },
            new()
            {
                Name = "Dermaplaning",
                Category = "Facials & Skin Care",
                Description = "Gentle exfoliation that removes dead skin cells and peach fuzz for a smooth glow. $40 add-on.",
                DurationMinutes = 30,
                Price = 40m,
                IsActive = true,
            },
            new()
            {
                Name = "Lymphatic Facial Massage",
                Category = "Facials & Skin Care",
                Description = "Gentle drainage massage to reduce puffiness and promote a healthy glow. $50 add-on.",
                DurationMinutes = 25,
                Price = 50m,
                IsActive = true,
            },
            new()
            {
                Name = "CoolSculpting",
                Category = "Body Treatments",
                Description = "Non-surgical fat reduction targeting stubborn areas resistant to diet and exercise. Contact for pricing.",
                DurationMinutes = 35,
                Price = 0m,
                IsActive = true,
                RequiredPriorSessionCount = 1,
            },
            new()
            {
                Name = "SkinPen Microneedling",
                Category = "Body Treatments",
                Description = "Collagen-induction therapy to improve skin texture, scars, and fine lines. Contact for pricing.",
                DurationMinutes = 45,
                Price = 0m,
                IsActive = true,
            },
            new()
            {
                Name = "Botox Cosmetic",
                Category = "Injectables",
                Description = "FDA-approved treatment to temporarily smooth moderate to severe frown lines. Featured — $99 intro rate for new clients. 18+ only.",
                DurationMinutes = 15,
                Price = 99m,
                IsActive = true,
                RequiresWaiverType = "MedicalIntake",
            },
            new()
            {
                Name = "Day of Bliss Package",
                Category = "Packages",
                Description = "A full day of pampering: Shellac manicure, pedicure, 50-min massage, facial, and body treatment. Contact for pricing.",
                DurationMinutes = 300,
                Price = 0m,
                IsActive = true,
            },
            new()
            {
                Name = "Break Time Package",
                Category = "Packages",
                Description = "A quick reset: spa pedicure paired with a 30-minute massage. Contact for pricing.",
                DurationMinutes = 90,
                Price = 0m,
                IsActive = true,
            },
            new()
            {
                Name = "Hot Yoga Class",
                Category = "Holistic/Wellness",
                Description = "A 60–75 minute heated yoga session to build strength, flexibility, and mindfulness. Contact for pricing.",
                DurationMinutes = 60,
                Price = 0m,
                IsActive = true,
            },
            new()
            {
                Name = "Float Tank Session",
                Category = "Holistic/Wellness",
                Description = "Sensory deprivation float therapy for deep relaxation and mental clarity. Contact for pricing.",
                DurationMinutes = 60,
                Price = 0m,
                IsActive = true,
            },
        };

        var existingNames = await db.Services.Select(s => s.Name).ToListAsync();
        var missing = services.Where(s => !existingNames.Contains(s.Name)).ToList();
        if (missing.Count > 0)
        {
            db.Services.AddRange(missing);
            await db.SaveChangesAsync();
        }
    }

    private static async Task SeedUsersAsync(AppDbContext db, IConfiguration configuration)
    {
        var hasher = new PasswordHasher<User>();
        var users = new List<User>
        {
            // Staff / providers
            NewSeedUser("Amara", "Johnson", "amara.johnson@fargospa.com", "701-555-0111", "Provider",
                "Massage Therapist", "12+ years in therapeutic and deep-tissue massage; specializes in sports recovery and chronic pain management."),
            NewSeedUser("Sofia", "Reyes", "sofia.reyes@fargospa.com", "701-555-0112", "Provider",
                "Esthetician", "Licensed esthetician; advanced facials, dermaplaning, skin rejuvenation."),
            NewSeedUser("Marcus", "Lee", "marcus.lee@fargospa.com", "701-555-0113", "Provider",
                "Medical Aesthetician", "Oversees all injectable and medical aesthetic treatments (Botox, CoolSculpting, microneedling)."),
            NewSeedUser("Priya", "Patel", "priya.patel@fargospa.com", "701-555-0114", "Provider",
                "Yoga & Wellness Instructor", "500-hour RYT certified; leads hot yoga and float tank sessions."),
            NewSeedUser("Hannah", "Bergstrom", "hannah.bergstrom@fargospa.com", "701-555-0115", "Provider",
                "Massage Therapist & Spa Coordinator", "Coordinates the Day of Bliss and Break Time packages."),

            // Clients
            NewSeedUser("Sarah", "Mitchell", "sarah.mitchell@example.com", "701-555-0201", "Client"),
            NewSeedUser("James", "Carter", "james.carter@example.com", "701-555-0202", "Client"),
            NewSeedUser("Maria", "Gonzalez", "maria.gonzalez@example.com", "701-555-0203", "Client"),

            // Testimonial authors
            NewSeedUser("Jessica", "Turner", "jessica.turner@example.com", "701-555-0301", "Client"),
            NewSeedUser("Michael", "Ramirez", "michael.ramirez@example.com", "701-555-0302", "Client"),
            NewSeedUser("Ashley", "Brooks", "ashley.brooks@example.com", "701-555-0303", "Client"),
            NewSeedUser("David", "Kim", "david.kim@example.com", "701-555-0304", "Client"),
            NewSeedUser("Emily", "Sanders", "emily.sanders@example.com", "701-555-0305", "Client"),
            NewSeedUser("Tyler", "Walsh", "tyler.walsh@example.com", "701-555-0306", "Client"),
            NewSeedUser("Lauren", "Hayes", "lauren.hayes@example.com", "701-555-0307", "Client"),
            NewSeedUser("Kevin", "Moore", "kevin.moore@example.com", "701-555-0308", "Client"),
        };

        var adminEmail = configuration["SeedAdmin:Email"] ?? "admin@fargospa.local";
        var adminPassword = configuration["SeedAdmin:Password"] ?? "DevAdmin123!Seed"; // dev-only default; override via SeedAdmin__Email / SeedAdmin__Password
        var admin = new User
        {
            FirstName = "Admin",
            LastName = "User",
            Email = adminEmail,
            Phone = "701-555-0100",
            Role = "Admin",
            CreatedAt = DateTime.UtcNow,
        };
        admin.PasswordHash = hasher.HashPassword(admin, adminPassword);
        users.Add(admin);

        var existingEmails = await db.Users.Select(u => u.Email).ToListAsync();
        var missing = users.Where(u => !existingEmails.Contains(u.Email)).ToList();
        if (missing.Count > 0)
        {
            db.Users.AddRange(missing);
            await db.SaveChangesAsync();
        }

        User NewSeedUser(string firstName, string lastName, string email, string phone, string role, string? title = null, string? bio = null)
        {
            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Role = role,
                Title = title,
                Bio = bio,
                PasswordHash = SeedPasswordHash,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }

    private static async Task SeedAppointmentsAsync(AppDbContext db)
    {
        if (await db.Appointments.AnyAsync())
        {
            return;
        }

        var services = await db.Services.ToDictionaryAsync(s => s.Name);
        var users = await db.Users.ToDictionaryAsync(u => u.Email);

        var sarah = users["sarah.mitchell@example.com"];
        var james = users["james.carter@example.com"];
        var maria = users["maria.gonzalez@example.com"];

        var amara = users["amara.johnson@fargospa.com"];
        var sofia = users["sofia.reyes@fargospa.com"];
        var marcus = users["marcus.lee@fargospa.com"];
        var priya = users["priya.patel@fargospa.com"];
        var hannah = users["hannah.bergstrom@fargospa.com"];

        var narrativeToday = new DateTime(2026, 8, 3);

        var bookings = new (User Client, Service Service, User Provider, DateTime Start)[]
        {
            (sarah, services["60-Min Therapeutic Massage"], amara, new DateTime(2026, 7, 4, 10, 0, 0)),
            (james, services["Deep Tissue Massage"], hannah, new DateTime(2026, 7, 6, 14, 0, 0)),
            (maria, services["Customized Facial"], sofia, new DateTime(2026, 7, 8, 11, 0, 0)),
            (sarah, services["Dermaplaning"], sofia, new DateTime(2026, 7, 12, 13, 0, 0)),
            (james, services["Salt Cave Massage"], amara, new DateTime(2026, 7, 15, 9, 0, 0)),
            (maria, services["Botox Cosmetic"], marcus, new DateTime(2026, 7, 18, 10, 30, 0)),
            (sarah, services["Hot Yoga Class"], priya, new DateTime(2026, 7, 22, 8, 0, 0)),
            (james, services["Float Tank Session"], priya, new DateTime(2026, 7, 25, 16, 0, 0)),
            (maria, services["Day of Bliss Package"], hannah, new DateTime(2026, 7, 29, 9, 0, 0)),
            (sarah, services["Lymphatic Facial Massage"], sofia, new DateTime(2026, 8, 1, 12, 0, 0)),
            (james, services["Break Time Package"], hannah, new DateTime(2026, 8, 5, 10, 0, 0)),
            (maria, services["60-Min Therapeutic Massage"], amara, new DateTime(2026, 8, 8, 15, 0, 0)),
        };

        var appointments = bookings.Select(b => new Appointment
        {
            ClientId = b.Client.Id,
            ServiceId = b.Service.Id,
            ProviderId = b.Provider.Id,
            StartTime = b.Start,
            EndTime = b.Start.AddMinutes(b.Service.DurationMinutes),
            CreatedAt = b.Start.AddDays(-3),
            Status = b.Start < narrativeToday ? "Completed" : "Upcoming",
        }).ToList();

        // A handful of bookings dated on the actual current day, with varied
        // statuses, so the Admin dashboard / Front Desk demo meaningfully
        // regardless of what day the app happens to be run on.
        var today = DateTime.Today;
        var todaysBookings = new (User Client, Service Service, User Provider, DateTime Start, string Status)[]
        {
            (sarah, services["Dermaplaning"], sofia, today.AddHours(9), "Completed"),
            (james, services["Deep Tissue Massage"], amara, today.AddHours(11), "CheckedIn"),
            (maria, services["Customized Facial"], sofia, today.AddHours(13), "NoShow"),
            (sarah, services["Hot Yoga Class"], priya, today.AddHours(15), "Upcoming"),
        };

        appointments.AddRange(todaysBookings.Select(b => new Appointment
        {
            ClientId = b.Client.Id,
            ServiceId = b.Service.Id,
            ProviderId = b.Provider.Id,
            StartTime = b.Start,
            EndTime = b.Start.AddMinutes(b.Service.DurationMinutes),
            CreatedAt = today.AddDays(-2),
            Status = b.Status,
        }));

        db.Appointments.AddRange(appointments);
        await db.SaveChangesAsync();
    }

    private static async Task SeedTestimonialsAsync(AppDbContext db)
    {
        if (await db.Testimonials.AnyAsync())
        {
            return;
        }

        var users = await db.Users.ToDictionaryAsync(u => u.Email);
        var services = await db.Services.ToDictionaryAsync(s => s.Name);

        var testimonials = new[]
        {
            new Testimonial
            {
                ClientId = users["jessica.turner@example.com"].Id,
                ServiceId = services["60-Min Therapeutic Massage"].Id,
                Rating = 5,
                ReviewText = "The 60-Min Therapeutic Massage was incredible — best massage I've ever had!",
                ApprovalStatus = "Approved",
                CreatedAt = new DateTime(2026, 6, 12),
            },
            new Testimonial
            {
                ClientId = users["michael.ramirez@example.com"].Id,
                ServiceId = services["Salt Cave Massage"].Id,
                Rating = 5,
                ReviewText = "The Salt Cave Massage is unlike anything else in town. Deeply relaxing.",
                ApprovalStatus = "Approved",
                CreatedAt = new DateTime(2026, 6, 18),
            },
            new Testimonial
            {
                ClientId = users["ashley.brooks@example.com"].Id,
                ServiceId = services["Customized Facial"].Id,
                Rating = 5,
                ReviewText = "My Customized Facial left my skin glowing for days. Sofia is amazing.",
                ApprovalStatus = "Approved",
                CreatedAt = new DateTime(2026, 6, 24),
            },
            new Testimonial
            {
                ClientId = users["david.kim@example.com"].Id,
                ServiceId = services["Botox Cosmetic"].Id,
                Rating = 4,
                ReviewText = "Botox Cosmetic results were subtle and natural-looking. Great experience with Dr. Lee.",
                ApprovalStatus = "Approved",
                CreatedAt = new DateTime(2026, 6, 30),
            },
            new Testimonial
            {
                ClientId = users["emily.sanders@example.com"].Id,
                ServiceId = services["Day of Bliss Package"].Id,
                Rating = 5,
                ReviewText = "The Day of Bliss Package was the perfect birthday treat — five hours of pure relaxation.",
                ApprovalStatus = "Approved",
                CreatedAt = new DateTime(2026, 7, 3),
            },
            new Testimonial
            {
                ClientId = users["tyler.walsh@example.com"].Id,
                ServiceId = services["Hot Yoga Class"].Id,
                Rating = 5,
                ReviewText = "Hot Yoga Class with Priya is intense but so rewarding. My flexibility has improved a ton.",
                ApprovalStatus = "Approved",
                CreatedAt = new DateTime(2026, 7, 8),
            },
            new Testimonial
            {
                ClientId = users["lauren.hayes@example.com"].Id,
                ServiceId = services["Dermaplaning"].Id,
                Rating = 4,
                ReviewText = "Dermaplaning made my skin so smooth. My makeup goes on flawlessly now.",
                ApprovalStatus = "Approved",
                CreatedAt = new DateTime(2026, 7, 12),
            },
            new Testimonial
            {
                ClientId = users["kevin.moore@example.com"].Id,
                ServiceId = services["Float Tank Session"].Id,
                Rating = 5,
                ReviewText = "The Float Tank Session was the most relaxed I've felt in years. Highly recommend.",
                ApprovalStatus = "Approved",
                CreatedAt = new DateTime(2026, 7, 15),
            },
        };

        db.Testimonials.AddRange(testimonials);
        await db.SaveChangesAsync();
    }

    // US-405: a couple of clients with a waiver on file and one without, so the missing-waiver alert is demonstrable
    private static async Task SeedWaiversAsync(AppDbContext db)
    {
        if (await db.Waivers.AnyAsync())
        {
            return;
        }

        var users = await db.Users.ToDictionaryAsync(u => u.Email);

        var waivers = new[]
        {
            new Waiver
            {
                ClientId = users["sarah.mitchell@example.com"].Id,
                WaiverType = "MassageIntake",
                IsSigned = true,
                SignedAt = new DateTime(2026, 6, 1),
            },
            new Waiver
            {
                ClientId = users["maria.gonzalez@example.com"].Id,
                WaiverType = "MedicalIntake",
                IsSigned = true,
                SignedAt = new DateTime(2026, 6, 15),
            },
            // James Carter intentionally has no MassageIntake waiver on file, to demonstrate the alert.
        };

        db.Waivers.AddRange(waivers);
        await db.SaveChangesAsync();
    }

    // US-503: a couple of clients with active health flags, so the badge is demonstrable on the schedule view
    private static async Task SeedHealthFlagsAsync(AppDbContext db)
    {
        if (await db.ClientHealthFlags.AnyAsync())
        {
            return;
        }

        var users = await db.Users.ToDictionaryAsync(u => u.Email);

        var flags = new[]
        {
            new ClientHealthFlag
            {
                ClientId = users["james.carter@example.com"].Id,
                FlagType = "Allergy",
                Details = "Sensitive to almond and coconut-based massage oils — use fragrance-free lotion.",
                IsActive = true,
            },
            new ClientHealthFlag
            {
                ClientId = users["maria.gonzalez@example.com"].Id,
                FlagType = "BloodThinner",
                Details = "Client is on a blood-thinning medication — expect increased bruising risk with injectables.",
                IsActive = true,
            },
        };

        db.ClientHealthFlags.AddRange(flags);
        await db.SaveChangesAsync();
    }
}
