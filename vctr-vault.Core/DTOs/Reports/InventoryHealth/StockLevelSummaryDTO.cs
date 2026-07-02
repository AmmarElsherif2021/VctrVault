// Core/DTOs/Reports/InventoryHealth/StockLevelSummaryDTO.cs
// Snapshot of current on-hand quantities across all warehouses.
// Sourced from: Material + Warehouse join via StockTransaction aggregation.
namespace vctr_vault.Core.DTOs.Reports.InventoryHealth;
public record StockLevelSummaryDTO
{
    public required string MaterialId { get; init; }        
    public required  string MaterialName { get; init; }    // e.g. "Steel Rod 10mm"
    public required  string Category { get; init; }        // e.g. "Raw Materials"
    public required string WarehouseName { get; init; }   // e.g. "Cairo Central"
    public decimal OnHandQty { get; init; }      // e.g. 450.00
    public required string UnitOfMeasure { get; init; }   // e.g. "KG"
    public decimal UnitCost { get; init; }       // e.g. 12.50
    public decimal TotalValue { get; init; }     // e.g. 5625.00 (OnHandQty × UnitCost)
    public DateTime AsOf { get; init; }          // e.g. 2026-07-01
}