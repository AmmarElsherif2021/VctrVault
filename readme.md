# VctrVault

Simulation-driven inventory system for construction businesses — a focused C#/.NET solution for secure, directional stock flow and decision-support by simulation.

## Table of Contents
- [Project Purpose](#project-aim)
- [What this is (short)](#what-this-is-short)
- [Architectural approach](#architectural-approach)
- [Current project state](#current-project-state)
- [Key features & capabilities](#key-features--capabilities)
- [Repository layout (high level)](#repository-layout-high-level)
- [How to build & run (quick start)](#how-to-build--run-quick-start)
- [Getting started / developing locally](#getting-started--developing-locally)
- [Contributing & Attributions](#contributing--attributions)
- [License & Contact](#license--contact)

## Project Purpose
VctrVault helps construction teams plan, track, and simulate inventory flows across warehouses and sites. It combines a domain-first inventory core with a standalone simulation layer so teams can run what-if scenarios (lead time changes, demand variability, transfer policies) to reduce stockouts and cost.

## What this is (short)
A modular .NET/C# solution that separates domain (Core), business rules (Business), persistence (Data), simulation (Simulation), and a composition/runner (Console). The simulation component is designed to drive decision-making without tightly coupling to higher-level business services.

## Architectural approach
- Clear separation of concerns: Core defines domain models, DTOs, enums, and essential interfaces; Business implements domain behavior; Data contains persistence adapters; Simulation depends only on Core; Console acts as the composition root.
- Simulation isolation: the simulation project references Core only (no Business or Data), ensuring the engine operates on domain primitives and interfaces. This enables swapping repositories or persistence without changing simulation code.
- Composition root pattern: Console registers concrete implementations into a DI container and composes SimulationEngine and MonteCarloRunner alongside repositories and services. Example DI registrations (composition lives in Console):
  ```csharp
  // Example registrations from Console (composition root)
  services.AddScoped<IMaterialRepository, EfMaterialRepository>();
  services.AddScoped<IInventoryService, InventoryService>();
  services.AddSingleton<ISimulationClock, WallClock>();
  services.AddSingleton<IRandomSource>(_ => new SeededRandomSource(seed: 42));
  services.AddScoped<SimulationEngine>();
  services.AddScoped<MonteCarloRunner>();
  // For fast test run:
  services.AddScoped<IMaterialRepository, InMemoryMaterialRepository>();
  ```
- Pluggable persistence: the design allows CSV/JSON/in-memory or an EF (DB) provider to be injected without altering engine logic.
- Simulation primitives required in Core: ISimulationClock, IRandomSource, and a domain event marker (IDomainEvent) are identified as necessary abstractions so the Simulation engine can run reproducible, time-controlled experiments.

## Current project state
- Solution present with these top-level projects:
  - vctr-vault.Core — domain models, DTOs, enums, interfaces (TargetFramework observed: .NET 10).
  - vctr-vault.Business — business domain documentation and business-layer notes (contains Business-Appendix.md, BusinessLayer.md, StockTransaction.md).
  - vctr-vault.Data — persistence adapters (repository for DB/CSV etc.).
  - vctr-vault.Simulation — simulation engine and documentation (contains a detailed Readme describing constraints and composition).
  - vctr-vault.Console — composition root / CLI runner.
  - vctr-vault.slnx — solution file.
- Documentation: substantive design and domain documents exist under the Business and Simulation projects.
- Code scaffolding: core projects and csproj files are present and target modern .NET (net10.0). Some projects contain minimal class scaffolding and documentation rather than finished application surfaces (Console likely intended as the run entry).
- Work remaining (non-exhaustive):
  - Implement or confirm concrete repository providers and full persistence migrations.
  - Expand simulation runners and user-facing scenario tooling (UI/CLI endpoints).
  - Add integration tests and sample scenario files for Monte Carlo runs and reporting outputs.
  - Add a top-level README (this file) and ATTRIBUTIONS / CONTRIBUTING / LICENSE files if desired.

## Key features & capabilities
- Discrete-event and Monte Carlo simulation support (engine + runner).
- Domain model for materials, suppliers, warehouses, and transactions (receipts, issues, transfers, adjustments).
- Directional stock flow and traceability (batch/lot/serial concepts documented).
- Pluggable repositories for fast in-memory testing or EF-backed persistence.
- Configurable reorder and replenishment policies, safety buffers, and policy evaluation via simulation.

## Repository layout (high level)
```
vctr-vault.Core/         Core domain: DTOs, Entities, Enums, Interfaces, csproj
vctr-vault.Business/     Business rules, domain documentation, transaction logic
vctr-vault.Data/         Persistence adapters (DB/CSV/JSON/in-memory)
vctr-vault.Simulation/   Simulation engine and documentation (depends on Core only)
vctr-vault.Console/      Composition root (DI, runners, CLI)
vctr-vault.slnx          Solution file
```
How it fits together: Console composes the system at runtime, injecting chosen persistence and service implementations into the simulation engine and business services. Simulation reads domain primitives and interfaces from Core, runs replicas with a seeded random source, and reports KPIs without referencing Business implementation details.

## How to build & run (quick start)
Prerequisites:
- .NET SDK 7/8/10+ installed (project files indicate TargetFramework net10.0).
- Git to clone the repository.

Quick commands (from repo root):
```bash
git clone https://github.com/AmmarElsherif2021/VctrVault.git
cd VctrVault
dotnet restore
dotnet build -c Release
# Run Console project (if it exposes run entry; adjust path if necessary)
dotnet run --project vctr-vault.Console
# Or run specific simulator project if configured:
dotnet run --project vctr-vault.Simulation
```
Notes:
- The Simulation Readme documents that the simulation engine expects certain interfaces in Core (ISimulationClock, IRandomSource, IDomainEvent). Ensure those are implemented or mocked when running Monte Carlo scenarios.
- Check each project's folder for additional README or sample scenario files (e.g., vctr-vault.Simulation/Readme.md and vctr-vault.Business/* docs).

## Getting started / developing locally
- Read the domain and simulation docs: vctr-vault.Business/Business-Appendix.md and vctr-vault.Simulation/Readme.md to align with intended invariants and composition rules.
- Implement or point to a persistence provider (in-memory for testing, EF for integration).
- Use the Console project as composition root to wire concrete repository and service implementations, then run SimulationEngine / MonteCarloRunner.
- Add sample scenario JSON files (suggested directory: /scenarios/) and an output directory for reports (/output/).
- Run unit tests with:
  ```bash
  dotnet test
  ```
  (Add tests if tests/ is not present).

## Contributing & Attributions
Contributions welcome. Suggested workflow:
- Fork the repo, create a branch (feature/...), implement changes, add tests, and open a PR with a clear description and motivation.
- Keep API and domain invariants stable; when changing domain models, update Business-Appendix.md and Simulation Readme as part of the PR.

Attributions
- Please add an ATTRIBUTIONS.md file or update this README with:
  - Names and licenses of any third-party libraries used.
  - Sources for any sample data or external models.
  - Credit for any design or documentation contributions (authors, domain experts).
- Add a CONTRIBUTORS file or use the repository's Contributors list for author attribution.

Code of conduct & contributor expectations
- Follow clear, respectful communication on issues and PRs.
- Provide tests for new behavior and update docs when public behavior changes.

## License & Contact
- Add a LICENSE file to state the intended license (MIT is recommended if open-source permissive licensing is desired).
- For questions, open an issue in this repository.

---
