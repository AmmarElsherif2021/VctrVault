namespace vctr_vault.Core.Entities.Transactions
{
    /// Represents a physical or virtual storage location (warehouse, store, depot)
    /// where inventory items are held.
    
    public class Warehouse:BaseEntity
    {
       
        /// Short, human‑readable code for the warehouse (e.g., "WH‑NYC", "DC‑LA").
        
        public string Code { get; set; } = string.Empty;


        /// Full name of the warehouse (e.g., "New York Main Distribution Center").
        
        public string Name { get; set; } = string.Empty;

        // ==================== Location & Contact Details ====================


        /// Street address, city, state, postal code, and country.
        
        public string Address { get; set; } = string.Empty;


        /// Primary contact person for this warehouse.
        
        public string? ContactPerson { get; set; }


        /// Contact phone number for the warehouse.
        
        public string? Phone { get; set; }


        /// Contact email address for the warehouse.
        
        public string? Email { get; set; }

        // ==================== Operational Attributes ====================


        /// Type of warehouse (e.g., "Main", "Satellite", "Distribution", "Returns").
        
        public string? WarehouseType { get; set; }


        /// Maximum storage capacity (e.g., in square feet, cubic meters, or pallet slots).
        /// Nullable to allow unknown or unlimited capacity.
        
        public double? Capacity { get; set; }


        /// Indicates whether this warehouse is currently operational and accepting stock.
        
        public bool IsActive { get; set; } = true;


        /// Any additional notes regarding the warehouse (e.g., restricted access, 
        /// temperature control details).
        
        public string? Notes { get; set; }


        // ==================== Navigation Properties (Relationships) ====================


        /// All stock transactions where this warehouse is the SOURCE (i.e., stock is
        /// moving OUT of this warehouse). Corresponds to StockTransaction.SourceWarehouseId.
        /// Marked as 'virtual' to enable EF Core Lazy Loading if configured.
        
        public virtual ICollection<StockTransaction> OutgoingTransactions { get; set; } 
            = new List<StockTransaction>();


        /// All stock transactions where this warehouse is the DESTINATION (i.e., stock is
        /// moving INTO this warehouse). Corresponds to StockTransaction.DestinationWarehouseId.
        /// Marked as 'virtual' to enable EF Core Lazy Loading if configured.
        
        public virtual ICollection<StockTransaction> IncomingTransactions { get; set; } 
            = new List<StockTransaction>();

        // For a simpler setup (if you don't need to distinguish inbound/outbound),
        // you could combine them into a single collection, but separating them 
        // makes querying much more intuitive.
    }
}