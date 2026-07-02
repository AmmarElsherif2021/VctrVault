namespace vctr_vault.Core.Entities
{
    public abstract class BaseEntity
    {
        public string Id= Guid.NewGuid().ToString();
        
        // ==================== Audit & Tracking ================
         public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// Date and time (UTC) when this supplier record was last updated.
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    }
}