using vctr_vault.Core.Enums;

namespace vctr_vault.Core.Entities.Transactions
{
    public abstract class StockTransaction : BaseEntity
    {
        // Mandatory properties (must be set by derived classes)
        public TransType TransactionType { get; protected set; }
        public string MaterialId { get; private set; }
        public decimal Quantity { get; private set; }
        public string UnitOfMeasure { get; private set; }

        // Optional/Additional properties
        public RefNumType? ReferenceNumber { get; set; }
        public TransState Phase { get; set; } = TransState.Pending;
        public string? CreatedBy { get; set; }
        public virtual Material? Material { get; set; }
        public decimal? UnitCost { get; set; }
        public CurrencyCode CurrencyCode { get; set; } = CurrencyCode.USD;
        public string? Reason { get; set; }
        public string? Notes { get; set; }

        // Protected constructor to enforce mandatory data from derived classes
        protected StockTransaction(TransType transactionType, string materialId, decimal quantity, string unitOfMeasure)
        {
            TransactionType = transactionType;
            MaterialId = materialId ?? throw new ArgumentNullException(nameof(materialId));
            Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive.", nameof(quantity));
            UnitOfMeasure = unitOfMeasure ?? throw new ArgumentNullException(nameof(unitOfMeasure));
        }

        // Force derived classes to implement their own cost logic (removes the magic -1.0m)
        public abstract decimal CalculateTotalCost();
    }
}