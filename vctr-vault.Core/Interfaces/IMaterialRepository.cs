using System.Collections.Generic;
using System.Threading.Tasks;
using vctr_vault.Core.Entities;
namespace vctr_vault.Core.Interfaces
{

    /// Defines the data persistence contract for Material operations.
    /// The Data layer (vctr-vault.Data) will implement this.
    /// The Business layer (vctr-vault.Business) will depend only on this.
    public interface IMaterialRepository
    {
        // ========== Basic CRUD ==========
        
        
        /// Retrieves a single material by its unique Id.

        Task<Material> GetByIdAsync(string id);

        
        /// Retrieves all materials in the system.

        Task<IEnumerable<Material>> GetAllAsync();

        
        /// Persists a new material to the data store.

        Task AddAsync(Material material);

        
        /// Updates an existing material's state in the data store.
        /// (Used after business logic modifies the entity).

        Task UpdateAsync(Material material);

        
        /// Removes a material from the data store by its Id.

        Task DeleteByIdAsync(string id);

        // ========== Domain-Specific Queries (based on your Material properties) ==========

        
        /// Finds all materials belonging to a specific category.
        /// (e.g., "Raw Steel", "Electrical Components").

        Task<IEnumerable<Material>> GetByCategoryAsync(string category);

        
        /// Retrieves materials where the current stock level has dropped at or below the ReorderLevel.
        /// Used by the Business layer to generate automatic reorder alerts.

        Task<IEnumerable<Material>> GetMaterialsNeedingReorderAsync();

        
        /// Finds materials by a partial name match (for search functionalities).

        Task<IEnumerable<Material>> SearchByNameAsync(string nameFragment);
    }
}