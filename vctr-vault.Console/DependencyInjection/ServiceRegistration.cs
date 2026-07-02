// vctr-vault.Console/DependencyInjection/ServiceRegistration.cs
//
// PURPOSE: This is the Composition Root helper.
// It is the ONLY file in the entire solution that is allowed to know about
// both Core interfaces AND their concrete implementations from Data and Business.
// Every other layer is isolated behind its interface — the wiring happens here and only here.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Core — interface contracts (the "what")
using vctr_vault.Core.Interfaces;

// Data — concrete infrastructure (the "how" for storage)
using vctr_vault.Data.DatabaseContext;
using vctr_vault.Data.Repostories;

// Business — concrete orchestration (the "how" for rules)
using vctr_vault.Business.Services;

namespace vctr_vault.Console.DependencyInjection;

public static class ServiceRegistration
{
    // Extension method on IServiceCollection.
    // Follows ASP.NET Core convention — when Phase 2 adds a Web API host,
    // this method moves there unchanged. The registrations themselves never shift.
    //
    // IConfiguration is accepted as a parameter because the connection string
    // lives in appsettings.json — it is never hardcoded here.
    // ServiceRegistration wires things; it does not own configuration values.
    public static IServiceCollection AddVctrVaultServices(
        this IServiceCollection services,
        IConfiguration config)
    {

        // ── BLOCK 1: INFRASTRUCTURE ───────────────────────────────────────────
        //
        // DbContext is registered first because every repository depends on it.
        // Dependency order matters conceptually even though the container resolves
        // lazily — registering in dependency order makes the file readable as a
        // dependency graph from bottom (infrastructure) to top (services).
        //
        // Lifetime: Scoped.
        // AddDbContext defaults to Scoped — one context per logical operation.
        // It tracks entity changes across that operation then disposes.
        // This is the correct lifetime for a unit-of-work pattern.
        services.AddDbContext<VctrVaultDbContext>(options =>
            options.UseSqlServer(

                // Connection string read from IConfiguration — never hardcoded.
                // Key must match the key in vctr-vault.Console/appsettings.json.
                config.GetConnectionString("DefaultConnection"),

                sqlOptions =>
                    // Without this, EF Core assumes migrations live in the startup
                    // project (Console). We redirect it to Data — the correct owner
                    // of all persistence concerns including migration history.
                    sqlOptions.MigrationsAssembly("vctr-vault.Data")
            )
        );


        // ── BLOCK 2: DATA LAYER BINDINGS ─────────────────────────────────────
        //
        // This is Dependency Inversion fulfilled at the Composition Root.
        // Core declared IMaterialRepository. Data implemented SqlMaterialRepository.
        // Neither knows the other exists. This single line is the only coupling point.
        //
        // Lifetime: Scoped — must match VctrVaultDbContext.
        // SqlMaterialRepository receives VctrVaultDbContext via constructor injection.
        // If the lifetimes differed (e.g. Singleton repo, Scoped context),
        // ValidateScopes would throw at startup — catching the mismatch immediately.
        services.AddScoped<IMaterialRepository, SqlMaterialRepository>();


        // ── BLOCK 3: BUSINESS LAYER BINDINGS ─────────────────────────────────
        //
        // IMaterialService → MaterialService
        // MaterialService implements business rules, enrichment, and orchestration.
        // It declares IMaterialRepository in its constructor — DI resolves that
        // automatically from Block 2's registration at runtime.
        // MaterialService never calls new SqlMaterialRepository() directly.
        //
        // Lifetime: Scoped — MaterialService depends on IMaterialRepository (Scoped),
        // so it must be Scoped or shorter. Singleton here would be a lifetime violation.
        services.AddScoped<IMaterialService, MaterialService>();

        // IReportService → ReportService
        // ReportService is a separate concern from MaterialService:
        // read-only aggregations, snapshots, and projections.
        // It also depends on IMaterialRepository for data access —
        // resolved automatically from Block 2.
        // Kept in a separate binding to honour ISP:
        // consumers that only need reports declare IReportService,
        // not the full IMaterialService contract.
        
        //services.AddScoped<IReportService, ReportService>();


        // Fluent return — allows Program.cs to chain further Add* calls if needed.
        // e.g. services.AddVctrVaultServices(config).AddLogging();
        return services;
    }
}