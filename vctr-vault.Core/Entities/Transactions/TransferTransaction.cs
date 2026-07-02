using vctr_vault.Core.Enums;

namespace vctr_vault.Core.Entities.Transactions
{
 public class TransferTransaction: StockTransaction
    {   //Foreign keys and actors ............................
        public string? SourceWarehouseId { get; private set; }
        /// Foreign key to the warehouse/location to which stock is added (for incoming).
        public string? DestinationWarehouseId { get; private set; }
        /// Navigation properties for warehouses (if needed).
        public virtual Warehouse? SourceWarehouse { get; private set; }
        public virtual Warehouse? DestinationWarehouse { get; private set; }
        public TransferTransaction(TransType TransactionType, string MaterialId, decimal Quantity, string UnitOfMeasure, string SourceWarehouseId, string DestinationWarehouseId)
        :base(TransactionType, MaterialId, Quantity, UnitOfMeasure){
         if ((TransactionType & TransType.Neutral) == 0)
                throw new ArgumentException(
                    $"'{TransactionType}' is not a valid outgoing transaction type. " +
                    $"Allowed values are: Transfer.", 
                    nameof(TransactionType));  
        this.TransactionType = TransactionType;
        this.SourceWarehouseId=SourceWarehouseId;
        this.DestinationWarehouseId=DestinationWarehouseId; 
        }
        public override decimal CalculateTotalCost()
        {
            return UnitCost.HasValue ? UnitCost.Value*(decimal)Quantity:0;
        }
        
    }    
}