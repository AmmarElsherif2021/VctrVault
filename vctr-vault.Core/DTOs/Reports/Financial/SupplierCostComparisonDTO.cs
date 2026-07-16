using vctr_vault.Core.Enums;
namespace vctr_vault.Core.DTOs.Reports.Financial;
// Core/DTOs/Reports/Financial/SupplierCostComparisonDTO.cs
// Average unit cost per material across different suppliers.
// Directly feeds SuggestReceipts() — pick cheapest reliable source.

public record SupplierCostComparisonRequestDTO
{
    public required List<string> MaterialIDs { get; init; }            // e.g. Guid("e1f2a3...")
    public required DateTime StartDate { get; init; }          // e.g. 2026-01-01
    public required DateTime EndDate { get; init; }            // e.g. 2026-06-30
    public required List<string>? SuppliersIDs { get; init; }         // e.g. Guid("f2a3b4...") or null for all suppliers
    public required CurrencyCode Currency { get; init; }      // e.g. CurrencyCode.USD
}
public record SupplierCostComparisonReportDTO
{
    public required List<string> MaterialIDs { get; init; }            // e.g. Guid("e1f2a3...")
    public required List<string> MaterialNames { get; init; }        // e.g. "Hydraulic Seal Kit"
    public required List<string> SuppliersIDs { get; init; }            // e.g. Guid("f2a3b4...")
    public required List<string> SupplierNames { get; init; }        // e.g. "Gulf Parts Ltd."
    public required List<decimal> AvgUnitCosts { get; init; }        // e.g. 43.20
    public required decimal MinUnitCost { get; init; }        // e.g. 41.00
    public required decimal MaxUnitCost { get; init; }        // e.g. 46.50
    public required int TotalReceiptsAnalysed { get; init; }  // e.g. 7
    public required CurrencyCode Currency { get; init; }      // e.g. CurrencyCode.USD
}