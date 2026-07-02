using System.Threading.Tasks;
using System.Collections.Generic;
using vctr_vault.Core.Entities;
using vctr_vault.Core.Entities.Transactions;
namespace vctr_vault.Core.Interfaces
{
    public interface IInventoryService
    {
        //======================== Basic StockTransactions CRUD ========================
        Task<StockTransaction> GetByIdAsync(Guid id);
        Task<IEnumerable<StockTransaction>> GetAllAsync();
        Task CreateAsync(StockTransaction stockTransaction);
        Task DeleteAsync(StockTransaction stockTransaction);
        //======================== Domain specific queries============
        //============================================================
        //============================================================

        // Core Inventory Movements (The Transaction Workflow) >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //Process Receipt (Goods Inward):>>
                /*- Records a ReceiptTransaction (Purchase, Return, or Adjustment IN).
                - Automatically increases the CurrentStock of the targeted Material.
                - Updates the weighted average UnitPrice of the Material based on the new purchase cost.
                - Links the transaction to the DestinationWarehouse and optionally a Supplier.*/
            Task ProcessReciptAsync(ReceiptTransaction receiptTransaction);

            //Process Issue (Goods Outward):>>
                /*
                Records an IssueTransaction (Consumption, Supplier Return, or Adjustment OUT).
                Validates that the outgoing quantity does not exceed the CurrentStock minus the SafetyBuffer (enforcing the safety margin defined in Material).
                Decreases the CurrentStock of the targeted Material.
                Links the transaction to the SourceWarehouse and optionally a Customer.
                */
            Task ProcessIssueAsync(IssueTransaction issueTransaction);

            //Process Transfer (Warehouse-to-Warehouse): .........
            /*
            Executes a dual-transaction (an Issue from the Source and a Receipt to the Destination) in a single atomic business operation.
            Ensures stock is deducted from SourceWarehouseId and added to DestinationWarehouseId simultaneously.
            */
            Task ProcessTransferTransactionAsync(Guid sourceWarehouse, Guid destinationWarehouse);
        // Reorder & Procurement Automation >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //Generate Reorder Suggestions:...................
            Task <IEnumerable<ReceiptTransaction>> SuggestReceipts();
            //Create Purchase Order (from Reorder):...........
            Task <ReceiptTransaction> Reorder(ReceiptTransaction oldReceiptTrans);
        // Stock Validation & Safety Enforcement (Business Rules):
            //Validate Issue Feasibility: AvailableStock = CurrentStock - SafetyBuffer......... in the system
            Task<Boolean> ValidateIssueStock(IssueTransaction issueTransaction , Material material);
            
            //Validate cycle count: System stock qty vs manually inserted count for system...............
            Task<Boolean> ValidateSysCycleCount(Material material, decimal Count);
            Task<Boolean> ValidateWarehouseCycleCount(Material material,Guid warehouseId, decimal count);

        // Inventory Validation and Financial Metrics:
            /*Calculate Total Inventory Value:
              Sums the UnitPrice multiplied x CurrentStock for all materials in:
              1- a specific Warehouse or globally. 
              2- Provides the total financial worth of the warehouse stock
              .................................................................
            */
            Task<decimal> CalcTotalInventory();
            Task<decimal> CalcWarehouseTotalInventory(Guid warehouseId);

            // Calculate Cost of Goods Sold (COGS): Aggregates all issued transactions (Material out).
            Task<decimal> CalculateCOGS(DateTime startDate, DateTime endDate);

    }  
}