namespace vctr_vault.Core.DTOs.Reports.Operational;
// Core/DTOs/Reports/Operational/CycleCountVarianceDTO.cs
// Output of ValidateWarehouseCycleCount() and ValidateSysCycleCount().
// Shows where physical count differs from system record — shrinkage, errors, theft.

public record CycleCountVarianceDTO
{
    public required string MaterialId { get; init; }            // e.g. Guid("b4c5d6...")
    public required string MaterialName { get; init; }        // e.g. "Copper Wire 2.5mm"
    public required string WarehouseName { get; init; }       // e.g. "Alexandria Annex"
    public required decimal SystemQty { get; init; }          // e.g. 850.00
    public required decimal PhysicalCountQty { get; init; }   // e.g. 812.00
    public required decimal VarianceQty { get; init; }        // e.g. -38.00
    public required decimal VariancePct { get; init; }        // e.g. -4.47
    public required decimal VarianceValue { get; init; }      // e.g. -332.50
    public required DateTime CountDate { get; init; }         // e.g. 2026-06-30
}