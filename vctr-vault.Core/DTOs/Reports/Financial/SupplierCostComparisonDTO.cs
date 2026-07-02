using vctr_vault.Core.Enums;
namespace vctr_vault.Core.DTOs.Reports.Financial;
// Core/DTOs/Reports/Financial/SupplierCostComparisonDTO.cs
// Average unit cost per material across different suppliers.
// Directly feeds SuggestReceipts() — pick cheapest reliable source.

public record SupplierCostComparisonDTO
{
    public required string MaterialId { get; init; }            // e.g. Guid("e1f2a3...")
    public required string MaterialName { get; init; }        // e.g. "Hydraulic Seal Kit"
    public required string SupplierId { get; init; }            // e.g. Guid("f2a3b4...")
    public required string SupplierName { get; init; }        // e.g. "Gulf Parts Ltd."
    public required decimal AvgUnitCost { get; init; }        // e.g. 43.20
    public required decimal MinUnitCost { get; init; }        // e.g. 41.00
    public required decimal MaxUnitCost { get; init; }        // e.g. 46.50
    public required int TotalReceiptsAnalysed { get; init; }  // e.g. 7
    public required CurrencyCode Currency { get; init; }      // e.g. CurrencyCode.USD
}