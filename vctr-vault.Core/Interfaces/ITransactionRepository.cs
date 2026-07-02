using vctr_vault.Core.Entities.Transactions;

namespace vctr_vault.Core.Interfaces
{
    public interface ITransactionRepository
    {
        Task<StockTransaction> GetByIdAsync(string Id);
        Task AddAsync(StockTransaction transaction);
        Task UpdateAsync(StockTransaction transaction);
        Task<IEnumerable<StockTransaction>> GetByMaterialId(string materialId);
    }
}