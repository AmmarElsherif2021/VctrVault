namespace vctr_vault.Core.DTOs.Reports.Operational;
// Core/DTOs/Reports/Operational/WarehouseUtilisationDTO.cs
// Stock volume per warehouse relative to its defined capacity.
// Feeds space planning and transfer decisions.

public record WarehouseUtilisationDTO
{
    public required string WarehouseId { get; init; }           // e.g. Guid("a3b4c5...")
    public required string WarehouseName { get; init; }       // e.g. "Cairo Central"
    public required decimal TotalCapacity { get; init; }      // e.g. 10000.00
    public required decimal UsedCapacity { get; init; }       // e.g. 7340.00
    public required decimal FreeCapacity { get; init; }       // e.g. 2660.00
    public required decimal UtilisationPct { get; init; }     // e.g. 73.40
    public required DateTime AsOf { get; init; }              // e.g. 2026-07-01
}