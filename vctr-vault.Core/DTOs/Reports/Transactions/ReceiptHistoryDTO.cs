using vctr_vault.Core.Enums;
namespace vctr_vault.Core.DTOs.Reports.Transactions;
// Core/DTOs/Reports/Transactions/ReceiptHistoryDTO.cs
// All ReceiptTransactions over a date range.
// Filterable by Supplier, Material, or DestinationWarehouse.

public record ReceiptHistoryDTO
{
    public required string TransactionId { get; init; }         // e.g. Guid("f6a7b8...")
    public required string ReferenceNumber { get; init; }     // e.g. "RCP-2026-00412"
    public required string MaterialName { get; init; }        // e.g. "Steel Rod 10mm"
    public required string SupplierName { get; init; }        // e.g. "Delta Industrial Co."
    public required string DestinationWarehouse { get; init; }// e.g. "Cairo Central"
    public required decimal Quantity { get; init; }           // e.g. 500.00
    public required decimal UnitCost { get; init; }           // e.g. 12.50
    public required decimal TotalCost { get; init; }          // e.g. 6250.00
    public required CurrencyCode Currency { get; init; }      // e.g. CurrencyCode.USD
    public required string State { get; init; }               // e.g. "Completed"
    public required DateTime TransactionDate { get; init; }   // e.g. 2026-06-28
}