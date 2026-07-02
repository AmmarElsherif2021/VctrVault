namespace vctr_vault.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IMaterialRepository Materials { get; }
        ITransactionRepository Transactions { get; }
        Task<int> SaveChangesAsync(); // Commit all changes atomically
    }
}