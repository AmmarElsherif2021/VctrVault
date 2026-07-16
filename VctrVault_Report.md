# Concrete file listing (per your requested folders) with status (Done / Empty / Placeholder)

---

## vctr-vault.Core/DTOs

- **Alerts/** — directory exists (no files returned; **Placeholder / needs inspection**)
- **Materials/** — directory exists (no files returned; **Placeholder / needs inspection**)
- **Reports/** — directory exists (no files returned; **Placeholder / needs inspection**)

**Notes:**
- The DTOs directory structure is present, but no concrete DTO files were found in the listing for these subfolders.
- Report DTOs are **TODOs** to implement:
  - Inventory Health
  - Transaction History
  - Financial/Cost
  - Warehouse/Operational
  - Simulation reports

---

## vctr-vault.Console

- `Program.cs` — **Done** (app entry point)
- `DependencyInjection/ServiceRegistration.cs` — **Done** (populated, but currently references the incorrect namespace `Repostories`)
- `vctr-vault.Console.csproj` — **Done**
- `bin/` — present (build artifacts)
- `obj/` — present (build artifacts)

**Status summary:**  
Console composition root and DI helper are present and implemented. `ServiceRegistration` registers `DbContext` and binds `IMaterialRepository` → `SqlMaterialRepository` and `IMaterialService` → `MaterialService`; the `IReportService` binding is commented out.

---

## vctr-vault.Data/DatabaseContext

- `VctrVaultDbContext.cs` — **Done** (populated)
- `vctr-vault.Data.csproj` — **Done**
- `bin/`, `obj/` — present (build artifacts)

**Status summary:**  
DbContext exists and is wired for SQL Server (migrations assembly configured to `vctr-vault.Data`).

---

## vctr-vault.Data/Repostories _(note: folder name contains a typo)_

- `SqlMaterialRepository.cs` — **Done** (populated)

**Status summary:**  
Repository implementation for materials exists and uses `VctrVaultDbContext`.  
The folder is named **Repostories** (typo) — should be **Repositories** to match naming conventions and avoid confusing namespaces.
---

## Additional empty / placeholder files relevant to these areas

- `vctr-vault.Core/Interfaces/IReportService.cs` — file exists but is **empty** (size 0) → **Empty / TODO**
- `vctr-vault.Business/Services/ReportService.cs` — file exists but is **empty** (size 0) → **Empty / TODO**

---

# Checklist PR (draft) — rename typo + add TODOs in empty files

### PR title
```
fix: rename Repostories → Repositories; add TODOs and small skeletons for report services/DTOs
```

### Branch name (suggested)
```
chore/rename-repostories-add-report-todos
```

### Commit message (suggested)
```
Rename vctr-vault.Data/Repostories → vctr-vault.Data/Repositories; update namespaces/usings; add TODOs and minimal skeletons for IReportService and ReportService; add TODO note to Core/DTOs/Reports README.
```

### PR description (suggested)
> This PR renames the Data folder from **Repostories** → **Repositories** to correct the typo and align namespaces.  
> It updates the using statements in the console composition root.  
> It also adds TODO markers and small skeleton code to the currently empty `IReportService` and `ReportService` files and places a README/TODO in `Core/DTOs/Reports` to capture expected report DTOs.  
> No behavior changes beyond the rename and small skeletons.  
> Run `dotnet build` after applying to verify namespaces.

---

### Files to change (concrete list and suggested edits)

#### 1) Rename folder
- **From:** `vctr-vault.Data/Repostories/`
- **To:** `vctr-vault.Data/Repositories/`
- **Action:** move all files under `Repostories` into `Repositories` and update namespaces inside each file (`namespace vctr_vault.Data.Repostories` → `namespace vctr_vault.Data.Repositories`).
- **Example rename required:**
  - `vctr-vault.Data/Repostories/SqlMaterialRepository.cs` → `vctr-vault.Data/Repositories/SqlMaterialRepository.cs`
  - Inside file change namespace line:
    ```diff
    - namespace vctr_vault.Data.Repostories
    + namespace vctr_vault.Data.Repositories
    ```

#### 2) Update Console composition root using statements
- **File:** `vctr-vault.Console/DependencyInjection/ServiceRegistration.cs`
- **Change:**
  ```diff
  - using vctr_vault.Data.Repostories;
  + using vctr_vault.Data.Repositories;
  ```
- Also update any commented binding or registrations referencing the old namespace if present.

#### 3) Add a short TODO / skeleton to Core IReportService
- **File:** `vctr-vault.Core/Interfaces/IReportService.cs` (currently empty)
- **Suggested content to add:**
  ```csharp
  // vctr-vault.Core/Interfaces/IReportService.cs
  // TODO: Define read-only report surface and DTO return types.

  namespace vctr_vault.Core.Interfaces
  {
      public interface IReportService
      {
          // Example: Task<IEnumerable<InventoryHealthReportDto>> GetInventoryHealthAsync();
          // TODO: add methods for TransactionHistory, Financial, WarehouseOperational, Simulation reports
      }
  }
  ```
- **Purpose:** Prevents a 0‑byte file and records the intended surface; replace comments with real DTOs & method signatures as Reports DTOs are implemented.

#### 4) Add a short TODO / skeleton to Business ReportService
- **File:** `vctr-vault.Business/Services/ReportService.cs` (currently empty)
- **Suggested content to add:**
  ```csharp
  // vctr-vault.Business/Services/ReportService.cs
  // TODO: Implement IReportService. Use IMaterialRepository and other repositories to aggregate read-only reports.

  using vctr_vault.Core.Interfaces;

  namespace vctr_vault.Business.Services
  {
      public class ReportService : IReportService
      {
          // TODO: add constructor injection for needed repositories (IMaterialRepository, ITransactionRepository, etc.)
          // TODO: implement methods to return report DTOs.
          // Temporary placeholder to avoid empty file during builds.
      }
  }
  ```

#### 5) Add a small README/TODO marker in DTOs/Reports
- **File:** `vctr-vault.Core/DTOs/Reports/README.md` (new)
- **Suggested content:**
  ```markdown
  # Reports DTOs
  TODO: Add report DTO records for:
  - Inventory Health
  - Transaction History
  - Financial/Cost
  - Warehouse/Operational
  - Simulation reports

  Suggested path: add C# records under this folder, e.g. `InventoryHealthReportDto.cs`
  ```

---

### Minimal diffs (example snippets)

**ServiceRegistration.cs** (change only the using)
```diff
- using vctr_vault.Data.Repostories;
+ using vctr_vault.Data.Repositories;
```

**SqlMaterialRepository.cs** (namespace change)
```diff
- namespace vctr_vault.Data.Repostories
+ namespace vctr_vault.Data.Repositories
```

**IReportService.cs** (example TODO skeleton)
```csharp
namespace vctr_vault.Core.Interfaces
{
    /// TODO: Define report contracts. Replace with real DTO return types.
    public interface IReportService
    {
        // Task<IEnumerable<InventoryHealthReportDto>> GetInventoryHealthAsync();
    }
}
```

**ReportService.cs** (example TODO skeleton)
```csharp
using vctr_vault.Core.Interfaces;

namespace vctr_vault.Business.Services
{
    public class ReportService : IReportService
    {
        // TODO: implement report aggregation methods.
    }
}
```

**Core/DTOs/Reports/README.md** — new small file (contents as above)

---

### Post-PR checklist (what to run after applying the PR)

- `dotnet build` (solution) — ensure namespace changes compile
- Search the repo for `"Repostories"` to confirm no remaining references (IDE replace or grep)
- Run any unit tests (if present)
- Confirm `ServiceRegistration` `AddScoped` for `IReportService` is uncommented once implementation is complete:
  ```csharp
  services.AddScoped<IReportService, ReportService>();
  ```

---

### Notes and rationale (concise)

- **Renaming the folder** corrects the namespace and prevents confusing spellings in code and DI registrations.
- **Adding TODO skeletons** prevents empty/zero‑length files, makes developer intent explicit, and allows builds to fail fast if other parts expect the interface to exist.
- **Adding a small README in Reports DTOs** centralizes what DTOs are needed and helps the next contributor implement them consistently.