namespace vctr_vault.Core.DTOs.Reports.Operational;
// Core/DTOs/Reports/Operational/InboundOutboundFlowDTO.cs
// Receipt vs Issue volume per warehouse over a period.
// Net flow = Inbound - Outbound; negative = warehouse is draining.

public record InOutFlowDTO
{
    public required string WarehouseId { get; init; }           // e.g. Guid("c5d6e7...")
    public required string WarehouseName { get; init; }       // e.g. "Cairo Central"
    public required decimal TotalInboundQty { get; init; }    // e.g. 4200.00
    public required decimal TotalOutboundQty { get; init; }   // e.g. 3750.00
    public required decimal NetFlowQty { get; init; }         // e.g. +450.00
    public required decimal TotalInboundValue { get; init; }  // e.g. 52500.00
    public required decimal TotalOutboundValue { get; init; } // e.g. 46875.00
    public required DateTime PeriodStart { get; init; }       // e.g. 2026-06-01
    public required DateTime PeriodEnd { get; init; }         // e.g. 2026-06-30
}