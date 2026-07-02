using System.Transactions;
using vctr_vault.Core.Entities.Transactions;

namespace vctr_vault.Core.Interfaces
{
    public interface IIssueTransactionService
    {
        Task<IssueTransaction> CreateIssueTransactionAsync(IssueTransaction transaction);
        Task ProcessTransaction(IssueTransaction transaction);
        Task<bool> ValidateTransaction(IssueTransaction transaction);
    }
    public interface IReceiptTransaction
    {
        Task<ReceiptTransaction> CreateReceiptTransaction(ReceiptTransaction transaction);
        Task ProcessReceiptTtransaction(ReceiptTransaction transaction);
        Task<bool> ValidateReceiptTransaction(ReceiptTransaction Transaction);
    }
    public interface ITransferTransaction
    {
        Task<TransferTransaction> CreateTransferTransaction(TransferTransaction transaction);
        Task ProcessTransferTransaction(TransferTransaction transaction);
        Task<bool> ValidateTransferTransaction(TransferTransaction transaction);
    }
}