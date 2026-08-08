# Fargo Spa & Wellness Center

A full-service spa/wellness booking platform built with ASP.NET Core Blazor (Interactive Server render mode) and EF Core over SQLite. Covers service browsing, online booking, staff/role management, tiered pricing, and admin front-desk operations. This README reflects the state of the codebase as of the latest pull from `main` (commit `e454088`).

## Current Status

**Not currently buildable on this machine.** The project targets `net10.0` (see `Family_and_Spa_Wellness.csproj`), but only the **.NET 8 SDK (8.0.422)** is installed locally. `dotnet build` fails immediately with:

```
NETSDK1045: The current .NET SDK does not support targeting .NET 10.0.
```

This is not a regression from the latest merge — the project has targeted `net10.0` since the file was first committed. To run the app, install the **.NET 10 SDK** (https://dotnet.microsoft.com/download/dotnet/10.0, or `winget install Microsoft.DotNet.SDK.10`), then `dotnet build` / `dotnet run` should work as described below.

The working tree is otherwise clean — no uncommitted changes.

## Tech Stack

- **Framework:** ASP.NET Core Blazor, Interactive Server render mode (`AddRazorComponents().AddInteractiveServerComponents()`)
- **Target framework:** `net10.0`
- **Database:** SQLite (`fargospa.db`, file-based, committed at repo root) via EF Core 10.0.10 (`Microsoft.EntityFrameworkCore.Sqlite`)
- **Auth:** Cookie authentication (`Microsoft.AspNetCore.Authentication.Cookies`), 14-day sliding expiration, custom `AdminBypassRolesHandler` that lets any user with the `Admin` claim bypass role-based authorization checks
- **Password hashing:** ASP.NET Core Identity's `PasswordHasher<User>` (used standalone — full Identity framework is not wired in)
- **Email:** `IEmailSender` abstraction with an `SmtpEmailSender` implementation, configured via `Smtp` section in `appsettings.json` (currently empty/unset — no real SMTP credentials configured)
- **Payments:** `IPaymentProvider` interface defined as a placeholder only — **no implementation exists yet**. Stubs correspond to backlog stories US-901 (charge), US-902 (transaction status), US-903 (refund)
- **EF tooling:** `dotnet-ef` 10.0.10, pinned via `dotnet-tools.json` (local tool manifest)

## Running the App

Two launch profiles are defined in `Properties/launchSettings.json`:

| Profile | URL(s) | Environment |
|---|---|---|
| `http` | http://localhost:5021 | Development |
| `https` | https://localhost:7125, http://localhost:5021 | Development |

On startup, `Program.cs` automatically runs `db.Database.MigrateAsync()` and then `SeedData.SeedAsync()` — so the SQLite database is created/migrated and seeded with demo data every time the app starts, no manual `dotnet ef database update` step required.

## Data Model

| Entity | Purpose | Key fields |
|---|---|---|
| `User` | Clients, providers, and admins (application-level identity, separate from `Staff`) | `Email` (unique), `PasswordHash`, `Role` (string: `Client`/`Provider`/`Admin`), `Title`/`Bio` (provider-only) |
| `Staff` | Internal staff roster used for the admin User Management screen — distinct table from `User` | `Role` (enum: `Administrator`/`Manager`/`Reception`/`Therapist`/`Viewer`), `IsActive` |
| `Service` | Spa service catalog | `Category`, `DurationMinutes`, `Price`, `IsActive` |
| `ServicePricingTier` | Optional per-duration and/or per-provider price override for a `Service` | `ServiceId`, `ProviderId` (nullable = global tier), `DurationMinutes`, `Price` |
| `Appointment` | Bookings | `ClientId`, `ServiceId`, `ProviderId` (nullable), `StartTime`/`EndTime`, `Status` (`Upcoming`/`CheckedIn`/`Completed`/`NoShow`/`Cancelled`) |
| `Testimonial` | Client reviews | `Rating` (1-5), `ReviewText`, `ApprovalStatus` (`Pending`/`Approved`/`Rejected`) |
| `ProviderAvailability` | Per-provider hourly availability overrides | `ProviderId`, `Date`, `Hour`, `IsAvailable` (presence of a row = marked unavailable) |

Note: **`User` and `Staff` are two separate, only loosely-linked tables** — `StaffService.UpdateRoleAsync` looks up a `User` by matching `Staff.Email` and keeps the `User.Role` string roughly in sync when a staff member's role enum changes, but there is no foreign key between them.

### Migrations (chronological)

1. `20260803210518_InitialCreate`
2. `20260803213236_AddUserTitleAndBio`
3. `20260803215805_AddAppointmentStatusTestimonialServiceAndAvailability`
4. `20260805230156_sqlite_migration_866`
5. `20260805231611_AddServicePricingTiers`

## Seed Data

`Data/SeedData.cs` seeds (idempotently, by checking for existing rows) on every app start:

- **13 services** across categories: Massage, Facials & Skin Care, Body Treatments, Injectables, Packages, Holistic/Wellness
- **16 users**: 5 providers (with `Title`/`Bio`), 3 general clients, 8 testimonial-author clients, plus a seeded **Admin** account
  - Admin credentials default to `admin@fargospa.local` / `DevAdmin123!Seed` unless overridden via config keys `SeedAdmin:Email` / `SeedAdmin:Password` (or env vars `SeedAdmin__Email` / `SeedAdmin__Password`)
  - Non-admin seed users have a placeholder password hash (`"seed-no-login"`) and **cannot log in** via the normal password form — they exist for display/relational purposes only
- **4 staff records** (Alice/Manager, Bob/Reception, Carol/Therapist, Dave/Viewer — Dave is seeded inactive) via `HasData` in `AppDbContext.OnModelCreating`
- **~16 appointments**: a fixed narrative set dated around a fictional "today" of 2026-08-03, plus a handful dated on the *actual* current day (`DateTime.Today`) with varied statuses (`Completed`, `CheckedIn`, `NoShow`, `Upcoming`) so the admin dashboard/front-desk views always have same-day data to show
- **8 approved testimonials**, one per client/service pairing

### Demo login

`POST /account/demo-login` (`Services/AccountAuthEndpoints.cs`) signs in as the first `User` matching a given role string, bypassing password checks entirely. Gated by config flag `DemoLogin:Enabled` (defaults to `true` — i.e. **enabled by default**, including presumably in production unless explicitly turned off).

## Routes / Pages

**Public:**
`/` (Home), `/about`, `/services`, `/providers`, `/testimonials`, `/membership`, `/gift-cards`, `/policies`, `/contact`, `/login`, `/register`, `/book`, `/consent-form`, `/access-denied`, `/not-found`, `/Error`

**Authenticated (client-facing):**
`/dashboard`, `/profile`, `/checkout`

**Admin** (all under `/admin`, all carry an authorization attribute):
`/admin` (AdminHome), `/admin/front-desk`, `/admin/my-calendar`, `/admin/checkout`, `/admin/fees`, `/admin/service-notes`, `/admin/services/pricing/{ServiceId:int}` (ServicePricing), `/admin/gift-cards`, `/admin/waivers`

**Other authorized page:** `/user-management` (top-level, not under `/admin`)

## Recent Work (per commit history)

- `e454088` — **US-201/US-202**: Admin service catalog CRUD (add/edit/deactivate) + tiered pricing by duration/provider (`ServicePricing.razor`, `PricingService`)
- `3a06015` — **US-106/US-205**: Staff role/access management and staff account CRUD (`UserManagement.razor`, `StaffService`, `Staff`/`Role` models)
- `6b9e160`, `8c60bfd` — Full page scaffold and styling to match the team's Bolt UI reference; registration flow; initial seed data
- `f788a1b` — Foundation matching team architecture

## Known Gaps / Not Yet Implemented

- **Payment processing**: `IPaymentProvider` is an unimplemented interface — no Stripe/Square (or other) integration exists. `/checkout` and `/admin/checkout` pages exist but cannot process real payments.
- **SMTP email**: `SmtpEmailSender` exists but `appsettings.json` has empty `Smtp` credentials — emails will fail to send until configured.
- **Local build blocked**: see "Current Status" above — needs the .NET 10 SDK installed.
- **`User` vs `Staff` duality**: two separate tables represent "who works here," kept manually in sync by email match — worth noting for anyone extending auth/roles.
