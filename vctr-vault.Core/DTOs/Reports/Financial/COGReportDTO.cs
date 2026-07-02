using vctr_vault.Core.Enums;
namespace vctr_vault.Core.DTOs.Reports.Financial;
// Core/DTOs/Reports/Financial/COGSReportDTO.cs
// Cost of Goods Sold per material over a date range — output of CalculateCOGS().
// Core financial metric: what did issued stock actually cost the business.

public record COGSReportDTO
{
    public required string MaterialId { get; init; }            // e.g. Guid("d0e1f2...")
    public required string MaterialName { get; init; }        // e.g. "Steel Rod 10mm"
    public required string Category { get; init; }            // e.g. "Raw Materials"
    public required decimal TotalQtyIssued { get; init; }     // e.g. 1200.00
    public required decimal WeightedAvgCost { get; init; }    // e.g. 12.75
    public required decimal TotalCOGS { get; init; }          // e.g. 15300.00
    public required CurrencyCode Currency { get; init; }      // e.g. CurrencyCode.USD
    public required DateTime PeriodStart { get; init; }       // e.g. 2026-06-01
    public required DateTime PeriodEnd { get; init; }         // e.g. 2026-06-30
}