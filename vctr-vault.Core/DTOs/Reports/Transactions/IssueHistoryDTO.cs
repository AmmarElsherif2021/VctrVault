using vctr_vault.Core.Enums;
namespace vctr_vault.Core.DTOs.Reports.Transactions;
// Core/DTOs/Reports/Transactions/IssueHistoryDTO.cs
// All IssueTransactions — tracks what left the warehouse and to whom.
// Useful for demand analysis and customer fulfillment auditing.

public record IssueHistoryDTO
{
    public required string TransactionId { get; init; }         // e.g. Guid("a7b8c9...").ToString()
    public required string ReferenceNumber { get; init; }     // e.g. "ISS-2026-00198"
    public required string MaterialName { get; init; }        // e.g. "Hydraulic Seal Kit"
    public required string SourceWarehouse { get; init; }     // e.g. "Alexandria Annex"
    public required string CustomerName { get; init; }        // e.g. "Nile Contracting LLC"
    public required decimal Quantity { get; init; }           // e.g. 15.00
    public required decimal UnitCost { get; init; }           // e.g. 45.00
    public required decimal TotalCost { get; init; }          // e.g. 675.00
    public required CurrencyCode Currency { get; init; }      // e.g. CurrencyCode.EGP
    public required string State { get; init; }               // e.g. "Completed"
    public required DateTime TransactionDate { get; init; }   // e.g. 2026-06-30
}