namespace vctr_vault.Core.Entities
{

    /// Represents a supplier entity in the small inventory system.
    /// Stores master data, procurement, logistics, financial, contractual, 
    /// quality, and compliance information.

    public class Supplier(string Name,string Email, string Phone, string Address, string ContactPerson):BaseEntity
    {
        // ==================== Supplier Master Data ====================

        public string Name { get; set; } = Name;
        public string Email { get; set; } = Email;
        public string Phone { get; set; } = Phone;
        public string Address { get; set; } = Address;
        public string ContactPerson { get; set; } = ContactPerson;

        // ==================== Procurement & Logistics Data ====================
        public int? LeadTimeDays { get; set; }
        public string? ShippingMethod { get; set; }
        public int? MinOrderQty { get; set; }
        public int? MaxOrderQty {get; set;}

        // ==================== Financial & Contractual Data ====================

        public string? TaxId { get; set; }

        /// Payment terms (e.g., "Net 30", "2/10 Net 60").
        public string? PaymentTerms { get; set; }

        /// Currency code used for transactions (ISO 4217, e.g., "USD", "EUR").
        public string? CurrencyCode { get; set; }

        /// Indicates whether the supplier is currently active for new orders.
        public bool IsActive { get; set; } = true;

        // ==================== Quality & Compliance Data ====================

        /// Average quality rating (e.g., 1–5 stars or percentage score).
        public double? QualityRating { get; set; }

        /// Certification or compliance standards met (e.g., "ISO 9001", "FDA").
        public string? ComplianceCertifications { get; set; }

        /// Any additional notes regarding quality or compliance issues.
        public string? QualityNotes { get; set; }

        
    }
}