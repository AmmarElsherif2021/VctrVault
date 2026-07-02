using vctr_vault.Core.Enums;
namespace vctr_vault.Core.DTOs.Reports.Financial;
// Core/DTOs/Reports/Financial/CurrencyExposureDTO.cs
// Total spend grouped by CurrencyCode — flags FX risk in procurement.
// Relevant when receipts span USD, EGP, EUR across different suppliers.

public record CurrencyExposureDTO
{
    public required CurrencyCode Currency { get; init; }      // e.g. CurrencyCode.USD
    public required decimal TotalSpend { get; init; }         // e.g. 184500.00
    public required int TransactionCount { get; init; }       // e.g. 34
    public required DateTime PeriodStart { get; init; }       // e.g. 2026-06-01
    public required DateTime PeriodEnd { get; init; }         // e.g. 2026-06-30
}