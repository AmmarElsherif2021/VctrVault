namespace vctr_vault.Core.DTOs.Reports.InventoryHealth;
// Core/DTOs/Reports/InventoryHealth/ReorderAlertDTO.cs
// Materials where OnHandQty <= ReorderPoint — feeds Reorder() and SuggestReceipts().
// Urgency is derived: Critical = below 50% of ReorderPoint, Warning = at or just above.
//Shortfall=Reorder Level−Current Stock. If Shortfall > 0 → Goods need to be reordered. If Shortfall ≤ 0 → Stock is sufficient (n

public record ReorderAlertDTO
{
    public required string MaterialId { get; init; }        // e.g. Guid("b2c3d4...")
    public required string MaterialName { get; init; }    // e.g. "Hydraulic Seal Kit"
    public required decimal OnHandQty { get; init; }      // e.g. 8.00
    public decimal ReorderPoint { get; init; }   // e.g. 20.00
    public decimal Shortfall { get; init; }      // e.g. 12.00 (ReorderPoint - OnHandQty)
    public required string UrgencyLevel { get; init; }    // e.g. "Critical" | "Warning"
    public required string PreferredSupplier { get; init; } // e.g. "Delta Industrial Co."
    public required DateTime AsOf { get; init; }          // e.g. 2026-07-01
}