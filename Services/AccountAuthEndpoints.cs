using System.Security.Claims;
using Family_and_Spa_Wellness.Data;
using Family_and_Spa_Wellness.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Family_and_Spa_Wellness.Services;

public static class AccountAuthEndpoints
{
    public static void MapAccountAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/account/login", async (
            HttpContext httpContext,
            AppDbContext db,
            [FromForm] string email,
            [FromForm] string password,
            [FromForm] string? returnUrl) =>
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            var hasher = new PasswordHasher<User>();

            if (user is null ||
                hasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                return Results.Redirect("/login?error=1");
            }

            await SignInAsync(httpContext, user);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Results.Redirect(returnUrl);
            }

            var redirectUrl = user.Role switch
            {
                "Admin" => "/admin",
                "Manager" => "/admin",
                "Provider" => "/dashboard",
                _ => "/dashboard",
            };

            return Results.Redirect(redirectUrl);
        });

        app.MapPost("/account/demo-login", async (
            HttpContext httpContext,
            AppDbContext db,
            IConfiguration configuration,
            [FromForm] string role,
            [FromForm] string? redirect) =>
        {
            var demoLoginEnabled = configuration.GetValue("DemoLogin:Enabled", true);
            if (!demoLoginEnabled)
            {
                return Results.NotFound();
            }

            var user = await db.Users.Where(u => u.Role == role).OrderBy(u => u.Id).FirstOrDefaultAsync();
            if (user is null)
            {
                return Results.Redirect("/login?error=1");
            }

            await SignInAsync(httpContext, user);

            var safeRedirect = !string.IsNullOrEmpty(redirect) && redirect.StartsWith('/') ? redirect : "/dashboard";
            return Results.Redirect(safeRedirect);
        });

        app.MapPost("/account/logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/");
        });
    }

    private static async Task SignInAsync(HttpContext httpContext, User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
        });
    }
}
