using Family_and_Spa_Wellness.Components;
using Family_and_Spa_Wellness.Data;
using Family_and_Spa_Wellness.Models;
using Family_and_Spa_Wellness.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.SectionName));
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, AdminBypassRolesHandler>();
builder.Services.AddCascadingAuthenticationState();
// Register ExportService
builder.Services.AddScoped<ExportService>();
builder.Services.AddScoped<IPaymentProvider, MockPaymentProvider>();
// Staff management service
builder.Services.AddScoped<StaffService>();
// Pricing service for tiered and provider-specific pricing
builder.Services.AddScoped<PricingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapAccountAuthEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await SeedData.SeedAsync(db, app.Configuration);
}

// Export endpoint: CSV download or printable HTML (for PDF via browser Print-to-PDF)
app.MapGet("/admin/export", async (HttpContext http, ExportService exporter) =>
{
    var q = http.Request.Query;
    var report = q["report"].ToString().ToLowerInvariant();
    var format = q["format"].ToString().ToLowerInvariant();
    if (string.IsNullOrEmpty(format)) format = "csv";

    DateTime start = DateTime.TryParse(q["start"], out var s) ? s : DateTime.UtcNow.Date.AddDays(-30);
    DateTime end = DateTime.TryParse(q["end"], out var e) ? e : DateTime.UtcNow.Date;

        if (format == "csv")
    {
        byte[] bytes;
        string filename;
        if (report == "revenue")
        {
            var res = await exporter.GenerateRevenueCsvAsync(start, end, "range");
            bytes = res.bytes; filename = res.filename;
        }
        else if (report == "staff")
        {
            var res = await exporter.GenerateStaffCsvAsync(start, end);
            bytes = res.bytes; filename = res.filename;
        }
        else if (report == "memberships")
        {
            var res = await exporter.GenerateMembershipCsvAsync(start, end);
            bytes = res.bytes; filename = res.filename;
        }
        else if (report == "operational")
        {
            var res = await exporter.GenerateOperationalCsvAsync(start, end);
            bytes = res.bytes; filename = res.filename;
        }
        else
        {
            // default to revenue
            var res = await exporter.GenerateRevenueCsvAsync(start, end, "range");
            bytes = res.bytes; filename = res.filename;
        }

        return Results.File(bytes, "text/csv", filename);
    }
    else
    {
        var html = await exporter.RenderReportHtmlAsync(report, start, end);
        return Results.Content(html, "text/html");
    }
});

app.Run();
