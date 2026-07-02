using vctr_vault.Core.Entities.Transactions;
using vctr_vault.Core.Enums;

namespace vctr_vault.Core.Entities
{
    /// <summary>
    /// Represents a manual stock adjustment (e.g., cycle count correction, damaged goods).
    /// Does not require Customer/Supplier/Warehouse context.
    /// </summary>
    public class AdjustmentTransaction : StockTransaction
    {
        public AdjustmentTransaction(
            TransType transactionType, 
            string materialId, 
            decimal quantity, 
            string unitOfMeasure,
            string reason) 
            : base(transactionType, materialId, quantity, unitOfMeasure)
        {
            Reason = reason ?? "Manual adjustment";
            
            // Adjustments are always neutral regarding external parties
            Phase = TransState.Completed; // Auto-complete adjustments
        }

        public override decimal CalculateTotalCost()
        {
            return UnitCost.HasValue ? UnitCost.Value * (decimal)Quantity : 0;
        }
    }
}