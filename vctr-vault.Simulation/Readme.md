The rule is simple and non-negotiable: `vctr-vault.Simulation` references `vctr-vault.Core` and nothing else. It never touches Business or Data directly. Here is how each concern resolves:

**Why Simulation → Core only**

The simulation needs entities (`Material`, `Warehouse`, `Supplier`), interfaces (`IInventoryService`, `IReplenishmentPolicy`, `ISimulationClock`, `IRandomSource`), and enums (`TransPhase`, `TransType`). All of these live in Core. The sim never needs to know whether `InventoryService` is backed by EF or SQLite — it calls `IInventoryService` and Console injects whichever concrete implementation is appropriate at runtime. This is DIP enforced structurally by project references, not just by discipline.

**Why Simulation cannot reference Business**

If Simulation referenced Business, you'd have a lateral dependency between two sibling projects that both reference Core. That's an architectural smell and it creates a coupling that eventually forces you to move things around when you add the Web API layer. More practically: the simulation should be swappable between a fast in-memory mode (for Monte Carlo speed) and a real EF-backed mode (for seeding from production data). That swap happens in Console at DI registration time — not inside Simulation itself.

**What Console does as Composition Root**

Console is the only project that sees everything. It registers the full DI container:

```
// real run — EF-backed
services.AddScoped<IMaterialRepository, EfMaterialRepository>();
services.AddScoped<IInventoryService, InventoryService>();
services.AddSingleton<ISimulationClock, WallClock>();
services.AddSingleton<IRandomSource>(_ => new SeededRandomSource(seed: 42));
services.AddScoped<SimulationEngine>();
services.AddScoped<MonteCarloRunner>();

// fast test run — swap repos only, engine unchanged
services.AddScoped<IMaterialRepository, InMemoryMaterialRepository>();
```

The engine and runner don't change. Only what gets injected changes. Console owns that decision.

**What needs to move into Core before you scaffold Simulation**

Three things must exist in Core first, or Simulation has nothing to depend on:

`ISimulationClock` — with `DateTimeOffset Now` and `void Advance(TimeSpan delta)`. Real implementation wraps `DateTimeOffset.UtcNow`. Sim implementation holds an internal mutable clock that events can advance.

`IRandomSource` — with `double NextDouble()` and `int Next(int max)`. Real implementation wraps `Random.Shared`. Sim implementation takes a seed at construction so each Monte Carlo replication is reproducible.

`IDomainEvent` — a marker interface. Every state change in Business raises a concrete domain event implementing this. The sim engine subscribes to these events to update its statistical accumulators without touching Business logic.

**The physical scaffolding steps**

Once those three interfaces are in Core, the sequence is:

First, add `vctr-vault.Simulation` to the solution — `dotnet new classlib -n vctr-vault.Simulation` then add it to `vctr-vault.slnx`. Add a project reference to `vctr-vault.Core` only.

Second, add `vctr-vault.Simulation` as a reference in `vctr-vault.Console`. Console is the only project that gains a new reference — nothing else changes.

Third, register `SimulationEngine` and `MonteCarloRunner` in Console's DI container alongside the existing service registrations.

Fourth, add a simulation menu option in Console that resolves `MonteCarloRunner` from the container and runs it. The existing console menus, the existing services, the existing repositories — none of them change.

**What this means for the Web API phase**

When you add `vctr-vault.Api` later, it slots in exactly where Console sits now — as a new Composition Root. It references Core, Data, Business, and Simulation. The simulation layer is already isolated and injectable, so the API can expose a `/simulate` endpoint without any restructuring. The dependency graph stays acyclic because Simulation never reaches up toward Console or Api.