namespace vctr_vault.Core.DTOs.Reports.InventoryHealth;
// Core/DTOs/Reports/InventoryHealth/OverStockDTO.cs
// Materials with on-hand stock exceeding a configurable threshold.
// Flags capital tied up in overstocked items.
public record OverStockRequestDTO

{
    public required decimal OverstockThreshold { get; init; }  // e.g. 1000.00
    public required List<string>? WarehouseIDs { get; init; } // e.g. Guid("a1b2c3...") or null for all warehouses
}
public record OverStockReportDTO
{
    public required List<string> MaterialIDs { get; init; }        // e.g. Guid("c3d4e5...")
    public required List<string> MaterialNames { get; init; }    // e.g. "Legacy Gasket V1"
    public required List<decimal> OnHandQtys { get; init; }      // e.g. 200.00
    public required List<decimal> StockValues { get; init; }     // e.g. 3400.00
    public required List<decimal> OverstockAmounts { get; init; } // e.g. 50.00
    public required List<string> WarehouseNames { get; init; }   // e.g. "Alexandria Annex"
}