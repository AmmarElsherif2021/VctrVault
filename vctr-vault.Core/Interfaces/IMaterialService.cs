using vctr_vault.Core.Entities;

namespace vctr_vault.Core.Interfaces
{
    
    /// Business logic and orchestration for Material.
    /// Does NOT duplicate IMaterialRepository's raw CRUD/Query methods.
    
    public interface IMaterialService
    {
        // =============================================
        // 1. BUSINESS WRITE OPERATIONS (with domain rules)
        // =============================================

        
        /// Creates a material after enforcing business rules:
        /// - Ensures Name is unique (calls repo to check).
        /// - Ensures UnitPrice > 0.
        /// - Automatically calculates SafetyBuffer and ReorderLevel.
        
        Task<Material> CreateMaterialAsync(Material material);

        
        /// Updates a material and triggers automatic domain recalculations.
        /// If ConsumptionRate or LeadTime changed, ReorderLevel is recomputed.
        
        Task<Material> UpdateMaterialAsync(Material material);

        
        /// Deletes a material ONLY IF CurrentStock is exactly 0.
        /// Prevents deleting inventory that still has financial value.
        
        Task<bool> DeleteMaterialAsync(string materialId);

        
        /// Orchestrates a stock adjustment.
        /// 1. Fetches Material via Repository.
        /// 2. Calls Material.UpdateStock() (domain logic).
        /// 3. Coordinates with ITransactionService to log the change.
        /// 4. Atomically saves via IUnitOfWork.
        
        Task<Material> AdjustStockAsync(string materialId, double adjustmentQty, string reason);

        // =============================================
        // 2. ENRICHED DOMAIN QUERIES (adds business context)
        // =============================================

        
        /// Retrieves a material by ID. Throws DomainNotFoundException if missing.
        /// (Repository returns null; Service adds the "throw" behavior).
        
        Task<Material> GetMaterialOrThrowAsync(string materialId);

        
        /// Enriches the Repository's GetMaterialsNeedingReorderAsync().
        /// Adds business context: Calculates ShortageQty and EstimatedDaysLeft 
        /// based on ConsumptionRate. Returns enriched DTO/Tuple.
        
        Task<IEnumerable<(Material Material, double ShortageQty, int EstimatedDaysLeft)>> 
            GetReorderRecommendationsWithContextAsync();

        
        /// Calculates the financial value of current stock (UnitPrice * CurrentStock).
        /// Pure business math—does not query the DB directly.
        
        Task<decimal> CalculateInventoryValueAsync(string materialId);

        // =============================================
        // 3. VALIDATION FOR OTHER SERVICES (Feasibility)
        // =============================================

        
        /// Checks if a requested quantity can be fulfilled.
        /// Used by IIssueTransactionService before creating an OUT transaction.
        
        Task<bool> IsStockAvailableAsync(string materialId, double requestedQty);

        
        /// Suggests an optimal purchase order quantity based on MOQ, 
        /// CurrentStock, and ConsumptionRate (business math).
        
        Task<int> SuggestOptimalOrderQuantityAsync(string materialId);
    }
}