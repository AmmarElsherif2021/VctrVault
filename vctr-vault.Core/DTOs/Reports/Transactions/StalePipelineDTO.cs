namespace vctr_vault.Core.DTOs.Reports.Transactions;
// Core/DTOs/Reports/Transactions/StalePipelineDTO.cs
// Transactions stuck in Pending or Processing beyond a threshold duration.
// Feeds operational alerts — stale transactions indicate broken workflow steps.

public record StalePipelineDTO
{
    public required string TransactionId { get; init; }         // e.g. Guid("c9d0e1...")
    public required string ReferenceNumber { get; init; }     // e.g. "RCP-2026-00389"
    public required string TransactionType { get; init; }     // e.g. "Receipt" | "Issue" | "Transfer"
    public required string MaterialName { get; init; }        // e.g. "Copper Wire 2.5mm"
    public required string CurrentPhase { get; init; }        // e.g. "Processing"
    public required DateTime CreatedAt { get; init; }         // e.g. 2026-06-22
    public required int HoursStuck { get; init; }             // e.g. 216  (9 days)
}