namespace vctr_vault.Core.DTOs.Reports.Transactionms;
// Core/DTOs/Reports/Transactions/TransferLogDTO.cs
// All TransferTransactions — inter-warehouse stock movements.
// Key for balancing stock distribution across locations.

public record TransferLogDTO
{
    public required string TransactionId { get; init; }         // e.g. Guid("b8c9d0...")
    public required string ReferenceNumber { get; init; }     // e.g. "TRF-2026-00055"
    public required string MaterialName { get; init; }        // e.g. "PVC Pipe 20mm"
    public required string SourceWarehouse { get; init; }     // e.g. "Cairo Central"
    public required string DestinationWarehouse { get; init; }// e.g. "Alexandria Annex"
    public required decimal Quantity { get; init; }           // e.g. 300.00
    public required string State { get; init; }               // e.g. "Completed"
    public required DateTime TransactionDate { get; init; }   // e.g. 2026-06-25
}