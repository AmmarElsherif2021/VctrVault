namespace vctr_vault.Core.DTOs.Reports.InventoryHealth;
// Core/DTOs/Reports/InventoryHealth/DeadStockDTO.cs
// Materials with zero transaction movement over a configurable lookback window.
// Flags capital tied up in non-moving stock.

public record DeadStockDTO
{
    public required string MaterialId { get; init; }        // e.g. Guid("c3d4e5...")
    public required string MaterialName { get; init; }    // e.g. "Legacy Gasket V1"
    public required decimal OnHandQty { get; init; }      // e.g. 200.00
    public required decimal StockValue { get; init; }     // e.g. 3400.00
    public required DateTime LastMovementDate { get; init; } // e.g. 2025-11-15
    public required int DaysSinceMovement { get; init; }  // e.g. 228
    public required string WarehouseName { get; init; }   // e.g. "Alexandria Annex"
}