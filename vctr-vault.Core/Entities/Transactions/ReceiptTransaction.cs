using System.Linq.Expressions;
using vctr_vault.Core.Enums;

namespace vctr_vault.Core.Entities.Transactions
{
 public class ReceiptTransaction: StockTransaction
    {   
        /// Foreign key to the Customer (for sales transactions).
        public string? SupplierId { get; private set; }
        /// Navigation property to the Supplier.
        public virtual Supplier? Supplier { get; private set; }
        

        /// Foreign key to the warehouse/location to which stock is added (for incoming).
        public string? DestinationWarehouseId { get; private set; }
        /// Navigation properties for warehouses (if needed).
        //public virtual Warehouse? SourceWarehouse { get; set; }
        public virtual Warehouse? DestinationWarehouse { get; set; }
        public ReceiptTransaction(TransType TransactionType, string MaterialId, decimal Quantity, string UnitOfMeasure, string SupplierId, string DestinationWarehouseId)
        :base(TransactionType, MaterialId, Quantity, UnitOfMeasure)
        {
         if ((TransactionType & TransType.In) == 0)
                throw new ArgumentException(
                    $"'{TransactionType}' is not a valid outgoing transaction type. " +
                    $"Allowed values are: Purchase, Return, Adjustment.", 
                    nameof(TransactionType));  
        this.TransactionType = TransactionType;
        this.SupplierId=SupplierId?? throw new ArgumentNullException(nameof(SupplierId)) ;
        this.DestinationWarehouseId=DestinationWarehouseId?? throw new ArgumentNullException(nameof(DestinationWarehouseId)); 
        }
        public override decimal CalculateTotalCost()
        {
          return UnitCost.HasValue ? UnitCost.Value * (decimal)Quantity : 0;
        }
        
    }    
}