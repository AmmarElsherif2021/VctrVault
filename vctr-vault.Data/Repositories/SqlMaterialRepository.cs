using Microsoft.EntityFrameworkCore;
using vctr_vault.Core.Entities;
using vctr_vault.Core.Interfaces;
using vctr_vault.Data.DatabaseContext;

namespace vctr_vault.Data.Repostories
{
public class SqlMaterialRepository: IMaterialRepository
    {
    // DbContext injected via constructor — DI supplies this at runtime.
    // SqlMaterialRepository never calls new VctrVaultDbContext(...) itself.
        private readonly string _Id;
        private readonly VctrVaultDbContext _context;
        public SqlMaterialRepository(VctrVaultDbContext context)
        {
            this._context=context;
        }
        public async Task<Material> GetByIdAsync(string id)
        {
            return await _context.Materials.FindAsync(id);
        }

        
        /// Retrieves all materials in the system.

        public async Task<IEnumerable<Material>> GetAllAsync()
        {
            return await _context.Materials.ToListAsync();
        }

        
        /// Persists a new material to the data store.

        public async Task AddAsync(Material material)
        {
         Console.WriteLine($"SqlMaterialRepo: Some Material with id {material.Id} should be added here.");   
         await _context.AddAsync(material);
        }

        
        /// Updates an existing material's state in the data store.
        /// (Used after business logic modifies the entity).

        public async Task UpdateAsync(Material material)
        {
            Console.WriteLine($"SqlMaterialRepo: Some Material with id {material.Id} should be updated here.");
            _context.Materials.Update(material);
            await _context.SaveChangesAsync();
        }

        
        /// Removes a material from the data store by its Id.

        public async Task DeleteByIdAsync(string id)
        {
            Console.WriteLine($"SqlMaterialRepo: Some Material with id: {id} should be deleted here.");
            var material = await _context.Materials.FindAsync(id);
            if (material is not null)
            {
                _context.Materials.Remove(material);
                await _context.SaveChangesAsync();
            }
        }

        // ========== Domain-Specific Queries (based on your Material properties) ==========

        
        /// Finds all materials belonging to a specific category.
        /// (e.g., "Raw Steel", "Electrical Components").

        public async Task<IEnumerable<Material>> GetByCategoryAsync(string category)
        {
            return await _context.Materials
                .Where(m => m.Category == category)
                .ToListAsync();
        }


        /// Retrieves materials where the current stock level has dropped at or below the ReorderLevel.
        /// Used by the Business layer to generate automatic reorder alerts.

        public async Task<IEnumerable<Material>> GetMaterialsNeedingReorderAsync()
         {
            return await _context.Materials.Where(m=>m.CurrentStock < m.MOQ).ToListAsync();
        }

        
        /// Finds materials by a partial name match (for search functionalities).

        public async Task<IEnumerable<Material>> SearchByNameAsync(string nameFragment)
         {
          return await _context.Materials.Where(m=>m.Name.Contains(nameFragment)).ToListAsync();
         }

    }    
}