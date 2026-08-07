using System.Text;
using Family_and_Spa_Wellness.Data;
using Family_and_Spa_Wellness.Models;
using Microsoft.EntityFrameworkCore;

namespace Family_and_Spa_Wellness.Services;

public class ExportService
{
    private readonly AppDbContext _db;

    public ExportService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(byte[] bytes, string filename)> GenerateRevenueCsvAsync(DateTime start, DateTime end, string period)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Revenue report,{start:yyyy-MM-dd},{end:yyyy-MM-dd},period={period}");

        // totals
        var dayTotal = await _db.Appointments.Where(a => a.Status == "Completed" && a.StartTime.Date == DateTime.UtcNow.Date).SumAsync(a => (decimal?)a.Price) ?? 0m;
        var weekStart = DateTime.UtcNow.Date.AddDays(-6);
        var weekTotal = await _db.Appointments.Where(a => a.Status == "Completed" && a.StartTime.Date >= weekStart).SumAsync(a => (decimal?)a.Price) ?? 0m;
        var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var monthTotal = await _db.Appointments.Where(a => a.Status == "Completed" && a.StartTime.Date >= monthStart).SumAsync(a => (decimal?)a.Price) ?? 0m;

        sb.AppendLine($"Totals,Day,{dayTotal}");
        sb.AppendLine($"Totals,Week,{weekTotal}");
        sb.AppendLine($"Totals,Month,{monthTotal}");
        sb.AppendLine();

        // service breakdown
        sb.AppendLine("Services,Revenue,Count");
        var svcGroups = await _db.Appointments.Where(a => a.Status == "Completed" && a.StartTime >= start && a.StartTime < end)
            .GroupBy(a => a.ServiceId)
            .Select(g => new { Id = g.Key, Total = g.Sum(a => a.Price), Count = g.Count() })
            .ToListAsync();

        var svcIds = svcGroups.Select(s => s.Id).ToList();
        var services = await _db.Services.Where(s => svcIds.Contains(s.Id)).ToDictionaryAsync(s => s.Id, s => s.Name);
        foreach (var g in svcGroups.OrderByDescending(x => x.Total))
        {
            services.TryGetValue(g.Id, out var name);
            sb.AppendLine($"\"{name ?? "(unknown)"}\",{g.Total},{g.Count}");
        }

        sb.AppendLine();
        sb.AppendLine("Providers,Revenue,Count");
        var provGroups = await _db.Appointments.Where(a => a.Status == "Completed" && a.ProviderId != null && a.StartTime >= start && a.StartTime < end)
            .GroupBy(a => a.ProviderId)
            .Select(g => new { Id = g.Key.Value, Total = g.Sum(a => a.Price), Count = g.Count() })
            .ToListAsync();
        var provIds = provGroups.Select(p => p.Id).ToList();
        var users = await _db.Users.Where(u => provIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.FullName);
        foreach (var g in provGroups.OrderByDescending(x => x.Total))
        {
            users.TryGetValue(g.Id, out var name);
            sb.AppendLine($"\"{name ?? "(unknown)"}\",{g.Total},{g.Count}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var filename = $"revenue_{start:yyyyMMdd}_{end:yyyyMMdd}.csv";
        return (bytes, filename);
    }

    public async Task<(byte[] bytes, string filename)> GenerateOperationalCsvAsync(DateTime start, DateTime end)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Operational report,{start:yyyy-MM-dd},{end:yyyy-MM-dd}");

        var appts = await _db.Appointments.Where(a => a.StartTime >= start && a.StartTime < end).ToListAsync();

        var total = appts.Count;
        var completed = appts.Count(a => a.Status == "Completed");
        var cancelled = appts.Count(a => a.Status == "Cancelled");
        var noshow = appts.Count(a => a.Status == "NoShow");

        sb.AppendLine($"TotalAppointments,{total}");
        sb.AppendLine($"Completed,{completed}");
        sb.AppendLine($"Cancelled,{cancelled}");
        sb.AppendLine($"NoShow,{noshow}");
        sb.AppendLine();

        sb.AppendLine("Date,Appointments,Completed,Cancelled,NoShow");
        var days = (end.Date - start.Date).Days + 1;
        for (int i = 0; i < days; i++)
        {
            var d = start.Date.AddDays(i);
            var dayAppts = appts.Where(a => a.StartTime.Date == d.Date).ToList();
            var t = dayAppts.Count;
            var c = dayAppts.Count(a => a.Status == "Completed");
            var x = dayAppts.Count(a => a.Status == "Cancelled");
            var n = dayAppts.Count(a => a.Status == "NoShow");
            sb.AppendLine($"{d:yyyy-MM-dd},{t},{c},{x},{n}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var filename = $"operational_{start:yyyyMMdd}_{end:yyyyMMdd}.csv";
        return (bytes, filename);
    }

    public async Task<(byte[] bytes, string filename)> GenerateStaffCsvAsync(DateTime start, DateTime end)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Staff report,{start:yyyy-MM-dd},{end:yyyy-MM-dd}");

        var providers = await _db.Users.Where(u => u.Role == "Provider").ToListAsync();
        var appts = await _db.Appointments.Where(a => a.ProviderId != null && a.StartTime >= start && a.StartTime < end && a.Status != "Cancelled").ToListAsync();
        var availEntries = await _db.ProviderAvailabilities.Where(pa => pa.Date >= start && pa.Date < end).ToListAsync();

        sb.AppendLine("Provider,BookedHours,AvailableHours,Revenue");
        foreach (var prov in providers)
        {
            var provAppts = appts.Where(a => a.ProviderId == prov.Id).ToList();
            var bookedHours = provAppts.Sum(a => (decimal)(a.EndTime - a.StartTime).TotalHours);
            var provAvail = availEntries.Where(pa => pa.ProviderId == prov.Id).ToList();
            decimal availableHours = provAvail.Any() ? provAvail.Count(pa => pa.IsAvailable) : ((end.Date - start.Date).Days + 1) * 8;
            var revenue = provAppts.Sum(a => a.Price);
            sb.AppendLine($"\"{prov.FullName}\",{bookedHours},{availableHours},{revenue}");
        }

        sb.AppendLine();
        sb.AppendLine("Appointments (Provider,Start,End,ClientId,ServiceId,Price)");
        foreach (var a in appts.OrderBy(a => a.ProviderId).ThenBy(a => a.StartTime))
        {
            sb.AppendLine($"{a.ProviderId},{a.StartTime:O},{a.EndTime:O},{a.ClientId},{a.ServiceId},{a.Price}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var filename = $"staff_report_{start:yyyyMMdd}_{end:yyyyMMdd}.csv";
        return (bytes, filename);
    }

    public async Task<(byte[] bytes, string filename)> GenerateMembershipCsvAsync(DateTime start, DateTime end)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Membership report,{start:yyyy-MM-dd},{end:yyyy-MM-dd}");

        var totalActive = await _db.Memberships.CountAsync(m => m.IsActive);
        var renewals = await _db.Memberships.CountAsync(m => m.LastRenewedDate >= start && m.LastRenewedDate <= end);
        var eligible = await _db.Memberships.CountAsync(m => m.EndDate >= start && m.EndDate <= end);
        var cancellations = await _db.Memberships.CountAsync(m => m.IsActive == false && m.EndDate >= start && m.EndDate <= end);

        sb.AppendLine($"TotalActive,{totalActive}");
        sb.AppendLine($"Renewals,{renewals}");
        sb.AppendLine($"EligibleForRenewal,{eligible}");
        sb.AppendLine($"Cancellations,{cancellations}");
        sb.AppendLine();

        sb.AppendLine("Date,ActiveCount");
        var days = (end.Date - start.Date).Days + 1;
        for (int i = 0; i < days; i++)
        {
            var d = start.AddDays(i);
            var cnt = await _db.Memberships.CountAsync(m => m.StartDate <= d && (m.EndDate == null || m.EndDate >= d));
            sb.AppendLine($"{d:yyyy-MM-dd},{cnt}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var filename = $"memberships_{start:yyyyMMdd}_{end:yyyyMMdd}.csv";
        return (bytes, filename);
    }

    public async Task<string> RenderReportHtmlAsync(string report, DateTime start, DateTime end)
    {
        // simple HTML representation for printing; include minimal styles and window.print()
        var sb = new StringBuilder();
        sb.AppendLine("<html><head><meta charset=\"utf-8\"><title>Report</title>");
        sb.AppendLine("<style>body{font-family:Arial,Helvetica,sans-serif;font-size:12px}table{border-collapse:collapse;width:100%}th,td{border:1px solid #ddd;padding:6px}</style>");
        sb.AppendLine("</head><body>");
        sb.AppendLine($"<h2>{report} report {start:yyyy-MM-dd} to {end:yyyy-MM-dd}</h2>");

        if (report == "revenue")
        {
            var (bytes, _) = await GenerateRevenueCsvAsync(start, end, "range");
            var csv = System.Text.Encoding.UTF8.GetString(bytes);
            sb.AppendLine("<pre>" + System.Net.WebUtility.HtmlEncode(csv) + "</pre>");
        }
        else if (report == "staff")
        {
            var (bytes, _) = await GenerateStaffCsvAsync(start, end);
            var csv = System.Text.Encoding.UTF8.GetString(bytes);
            sb.AppendLine("<pre>" + System.Net.WebUtility.HtmlEncode(csv) + "</pre>");
        }
        else if (report == "memberships")
        {
            var (bytes, _) = await GenerateMembershipCsvAsync(start, end);
            var csv = System.Text.Encoding.UTF8.GetString(bytes);
            sb.AppendLine("<pre>" + System.Net.WebUtility.HtmlEncode(csv) + "</pre>");
        }

        sb.AppendLine("<script>window.onload = function(){ window.print(); }</script>");
        sb.AppendLine("</body></html>");
        return sb.ToString();
    }
}
