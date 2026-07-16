namespace vctr_vault.Core.DTOs.Reports.InventoryHealth;
// Core/DTOs/Reports/InventoryHealth/DeadStockDTO.cs
// Materials with zero transaction movement over a configurable lookback window.
// Flags capital tied up in non-moving stock.

public record DeadStockRequestDTO
{
    public required DateTime LookbackStart { get; init; }     // e.g. 2025-01-01
    public required DateTime LookbackEnd { get; init; }       // e.g. 2025-12-31
    public required List<string>? WarehouseIDs { get; init; } // e.g. Guid("a1b2c3...") or null for all warehouses
}
public record DeadStockReportDTO
{
    public required List<string> MaterialIDs { get; init; }        // e.g. Guid("c3d4e5...")
    public required List<string> MaterialNames { get; init; }    // e.g. "Legacy Gasket V1"
    public required List<decimal> OnHandQtys { get; init; }      // e.g. 200.00
    public required List<decimal> StockValues { get; init; }     // e.g. 3400.00
    public required List<DateTime> LastMovementDates { get; init; } // e.g. 2025-11-15
    public required List<int> DaysSinceMovements { get; init; }  // e.g. 228
    public required List<string> WarehouseNames { get; init; }   // e.g. "Alexandria Annex"
}