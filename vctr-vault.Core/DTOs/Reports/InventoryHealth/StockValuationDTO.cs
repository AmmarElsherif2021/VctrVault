using vctr_vault.Core.Enums;
namespace vctr_vault.Core.DTOs.Reports.InventoryHealth;
// Core/DTOs/Reports/InventoryHealth/StockValuationDTO.cs
// Total monetary value of inventory per material.
// Feeds financial reporting and balance sheet inventory line.

public record StockValuationDTO
{
    public required string MaterialId { get; init; }        // e.g. Guid("e5f6a7...")
    public required string MaterialName { get; init; }    // e.g. "Copper Wire 2.5mm"
    public required string Category { get; init; }        // e.g. "Electrical"
    public required decimal TotalQty { get; init; }       // e.g. 3200.00
    public required decimal UnitCost { get; init; }       // e.g. 8.75
    public required decimal TotalValue { get; init; }     // e.g. 28000.00
    public required CurrencyCode Currency { get; init; }  // e.g. CurrencyCode.EGP
    public required DateTime ValuationDate { get; init; } // e.g. 2026-07-01
}