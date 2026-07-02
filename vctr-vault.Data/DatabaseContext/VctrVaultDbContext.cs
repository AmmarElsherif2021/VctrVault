// vctr-vault.Data/VctrVaultDbContext.cs
// EF Core DbContext lives in Data — Core never references EF.
// This is the infrastructure boundary: ORM details stay here, never leak upward.
//DbContext is your unit of work + repository combined:
//It tracks changes to entities.
//It executes queries and saves updates.
//It encapsulates database access logic so your business code stays clean and abstracted from raw SQL.
using Microsoft.EntityFrameworkCore;
using vctr_vault.Core.Entities;
using vctr_vault.Core.Entities.Transactions;
namespace vctr_vault.Data.DatabaseContext;

public class VctrVaultDbContext(DbContextOptions<VctrVaultDbContext> options) : DbContext(options)
{
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
}
