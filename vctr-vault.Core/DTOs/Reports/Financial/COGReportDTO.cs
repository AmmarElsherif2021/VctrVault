using vctr_vault.Core.Enums;
namespace vctr_vault.Core.DTOs.Reports.Financial;
// Core/DTOs/Reports/Financial/COGSReportDTO.cs
// Cost of Goods Sold per material over a date range — output of CalculateCOGS().
// Core financial metric: what did issued stock actually cost the business.

public record COGSRequestDTO
{
    public required DateTime StartDate { get; init; }          // e.g. 2026-01-01
    public required DateTime EndDate { get; init; }            // e.g. 2026-06-30
    public required string WarehouseId { get; init; }         // e.g. Guid("a1b2c3...") or null for all warehouses
    
    //search filters
    public string? MaterialId { get; init; }          // e.g. Guid("d0e1f2...") or null for all materials
    public string? Category { get; init; }            // e.g. "Raw Materials"
} 
public record COGSReportDTO
{
    public required string WarehouseId { get; init; }  // e.g. Guid("d0e1f2...")
    public required decimal TotalQtyIssued { get; init; }     // e.g. 1200.00
    public required decimal WeightedAvgCost { get; init; }    // e.g. 12.75
    //Filters
    public string? MaterialId { get; init; }            // e.g. Guid("a1b2c3...") or null for all materials
    public string? Category { get; init; }            // e.g. "Raw Materials"

    public required decimal TotalCOGS { get; init; }          // e.g. 15300.00
    
}