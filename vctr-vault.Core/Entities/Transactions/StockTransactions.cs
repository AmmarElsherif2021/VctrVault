using vctr_vault.Core.Enums;

namespace vctr_vault.Core.Entities.Transactions
{
    public abstract class StockTransaction(TransType transactionType, string materialId, decimal quantity, string unitOfMeasure) : BaseEntity
    {
        // Mandatory properties (must be set by derived classes)
        public TransType TransactionType { get; protected set; } = transactionType;
        public string MaterialId { get; private set; } = materialId ?? throw new ArgumentNullException(nameof(materialId));
        public decimal Quantity { get; private set; } = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        public string UnitOfMeasure { get; private set; } = unitOfMeasure ?? throw new ArgumentNullException(nameof(unitOfMeasure));

        // Optional/Additional properties
        public RefNumType? ReferenceNumber { get; set; }
        public TransState State { get; set; } = TransState.Pending;
        public string? CreatedBy { get; set; }
        public virtual Material? Material { get; set; }
        public decimal? UnitCost { get; set; }
        public CurrencyCode CurrencyCode { get; set; } = CurrencyCode.USD;
        public string? Reason { get; set; }
        public string? Notes { get; set; }

        // Force derived classes to implement their own cost logic (removes the magic -1.0m)
        public virtual decimal CalculateTotalCost()
        {
            return UnitCost.HasValue ? UnitCost.Value * Quantity : 0;
        }
    }
}