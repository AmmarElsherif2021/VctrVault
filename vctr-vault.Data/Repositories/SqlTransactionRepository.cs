using vctr_vault.Core.Interfaces;
using vctr_vault.Data.DatabaseContext;
using vctr_vault.Core.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
namespace vctr_vault.Data.Repostories
{
    public class SqlTransactionRepository: ITransactionRepository
    {
        // DbContext injected via constructor — DI supplies this at runtime.
        // SqlTransactionRepository never calls new VctrVaultDbContext(...) itself.
        private readonly string _Id;
        private readonly VctrVaultDbContext _context;
        public SqlTransactionRepository(VctrVaultDbContext context)
        {
            this._context=context;
        }
        public async Task<StockTransaction> GetByIdAsync(string Id)
        {
            return await _context.StockTransactions.FindAsync(Id);
            
        }
        public async Task AddAsync(StockTransaction transaction)
        {
            await _context.StockTransactions.AddAsync(transaction);
        }
        public async Task UpdateAsync(string Id, StockTransaction transaction)
        {
            StockTransaction existingTransaction = await _context.StockTransactions.FindAsync(transaction.Id);
            if (existingTransaction is null)
            {
                throw new InvalidOperationException($"Transaction with Id {transaction.Id} does not exist.");
            }
            _context.StockTransactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<StockTransaction>> GetByMaterialId(string materialId)
        {
            return await _context.StockTransactions.Where(t => t.MaterialId == materialId).ToListAsync<StockTransaction>();
        }
    }
}