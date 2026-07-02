using vctr_vault.Core.Enums;
namespace vctr_vault.Core.Entities.Transactions
{
    //IssueTransaction is a general substitution transaction process [OUT process]
    public class IssueTransaction:StockTransaction
    { 
        public string? SourceWarehouseId { get;private set; }
        /// Foreign key to the warehouse/location to which stock is added (for incoming).
        public string? CustomerId { get;private set; }
        /// Navigation properties for warehouses (if needed).
        // public virtual Warehouse? SourceWarehouse { get; set; }
        // public virtual Warehouse? DestinationWarehouse { get; set; }
        public IssueTransaction(TransType TransactionType,string MaterialId, decimal Quantity, string UnitOfMeasure, string SourceWarehouseId,string CustomerId)
        :base(TransactionType,MaterialId,Quantity,UnitOfMeasure)
        {  
            if ((TransactionType & TransType.Out) == 0)
                throw new ArgumentException(
                    $"'{TransactionType}' is not a valid outgoing transaction type. " +
                    $"Allowed values are: Consumption, SupplierReturn, Adjustment.", 
                    nameof(TransactionType));
            this.TransactionType=TransactionType;
            this.SourceWarehouseId= SourceWarehouseId;
            this.CustomerId=CustomerId;
        }
        public override decimal CalculateTotalCost()
        {
            return UnitCost.HasValue? UnitCost.Value*(decimal)Quantity:0;
        }
    }
}